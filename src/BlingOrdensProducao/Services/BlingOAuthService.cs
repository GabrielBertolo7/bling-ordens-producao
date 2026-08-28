using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BlingOrdensProducao.Models;

namespace BlingOrdensProducao.Services;

// Fluxo OAuth2 (Authorization Code) com a API v3 do Bling: autorização inicial via navegador e
// servidor local de loopback, e depois renovação automática do access_token via refresh_token.
public sealed class BlingOAuthService
{
    private const string AuthorizeUrl = "https://www.bling.com.br/Api/v3/oauth/authorize";
    private const string TokenUrl = "https://www.bling.com.br/Api/v3/oauth/token";

    // Precisa ser cadastrada exatamente assim (com a barra final) no painel de desenvolvedor do
    // Bling ao registrar o aplicativo.
    public const string RedirectUri = "http://localhost:8765/callback/";

    private static readonly TimeSpan TokenExpiryBuffer = TimeSpan.FromMinutes(2);
    private static readonly HttpClient HttpClient = new();

    private readonly SecureTokenStorage _storage;

    public BlingOAuthService(SecureTokenStorage storage)
    {
        _storage = storage;
    }

    public bool IsAuthorized => _storage.Exists();

    public void SignOut() => _storage.Clear();

    // Abre o navegador, sobe um listener local pra capturar o redirect, troca o código por tokens
    // e salva tudo criptografado.
    public async Task AuthorizeAsync(string clientId, string clientSecret, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new ArgumentException("Client ID e Client Secret são obrigatórios.");
        }

        var state = GenerateState();
        var authorizeRequestUrl = BuildAuthorizeUrl(clientId, state);

        var (code, error) = await CaptureAuthorizationCodeAsync(authorizeRequestUrl, state, cancellationToken);

        if (error is not null)
        {
            throw new BlingApiException($"O Bling recusou a autorização: {error}");
        }

        if (string.IsNullOrEmpty(code))
        {
            throw new BlingApiException("Nenhum código de autorização foi recebido.");
        }

        var tokenResponse = await ExchangeCodeForTokenAsync(clientId, clientSecret, code, cancellationToken);

        _storage.Save(new StoredAuthData
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            AccessTokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresInSeconds),
        });
    }

    // Retorna um access_token válido, renovando via refresh_token quando estiver perto de expirar.
    public async Task<string> GetValidAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var data = _storage.Load()
            ?? throw new BlingApiException("O aplicativo ainda não foi autorizado a acessar o Bling.");

        if (DateTime.UtcNow + TokenExpiryBuffer < data.AccessTokenExpiresAtUtc)
        {
            return data.AccessToken;
        }

        var tokenResponse = await RefreshTokenAsync(data.ClientId, data.ClientSecret, data.RefreshToken, cancellationToken);

        data.AccessToken = tokenResponse.AccessToken;
        data.RefreshToken = tokenResponse.RefreshToken;
        data.AccessTokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresInSeconds);
        _storage.Save(data);

        return data.AccessToken;
    }

    private static string BuildAuthorizeUrl(string clientId, string state)
    {
        var query = string.Join('&',
            "response_type=code",
            $"client_id={Uri.EscapeDataString(clientId)}",
            $"state={Uri.EscapeDataString(state)}",
            $"redirect_uri={Uri.EscapeDataString(RedirectUri)}");

        return $"{AuthorizeUrl}?{query}";
    }

    private static async Task<(string? Code, string? Error)> CaptureAuthorizationCodeAsync(
        string authorizeRequestUrl, string expectedState, CancellationToken cancellationToken)
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(RedirectUri);

        try
        {
            listener.Start();
        }
        catch (HttpListenerException ex)
        {
            throw new BlingApiException(
                $"Não foi possível abrir o servidor local em {RedirectUri}. " +
                "Verifique se outro programa não está usando essa porta.", ex);
        }

        try
        {
            Process.Start(new ProcessStartInfo(authorizeRequestUrl) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            listener.Stop();
            throw new BlingApiException("Não foi possível abrir o navegador para autorizar o aplicativo.", ex);
        }

        var contextTask = listener.GetContextAsync();
        var timeoutTask = Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);

        try
        {
            var completedTask = await Task.WhenAny(contextTask, timeoutTask);
            if (completedTask == timeoutTask)
            {
                throw new BlingApiException("Tempo esgotado esperando a autorização no navegador.");
            }

            var context = await contextTask;
            var query = context.Request.QueryString;
            var code = query["code"];
            var error = query["error"];
            var returnedState = query["state"];

            await RespondToBrowserAsync(context, success: error is null);

            if (error is null && returnedState != expectedState)
            {
                return (null, "estado (state) inválido, possível interferência de terceiros. Tente novamente.");
            }

            return (code, error);
        }
        finally
        {
            listener.Stop();
            // Sem isso, parar o listener com GetContextAsync ainda pendente gera uma exceção não observada.
            _ = contextTask.ContinueWith(t => _ = t.Exception, TaskScheduler.Default);
        }
    }

    private static async Task RespondToBrowserAsync(HttpListenerContext context, bool success)
    {
        var html = success
            ? "<html><head><meta charset='utf-8'></head><body><h2>Autorização concluída.</h2><p>Pode fechar esta aba e voltar para o aplicativo.</p></body></html>"
            : "<html><head><meta charset='utf-8'></head><body><h2>Falha na autorização.</h2><p>Volte ao aplicativo e tente novamente.</p></body></html>";

        var buffer = Encoding.UTF8.GetBytes(html);
        context.Response.ContentType = "text/html; charset=utf-8";
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer);
        context.Response.OutputStream.Close();
    }

    private static Task<BlingTokenResponse> ExchangeCodeForTokenAsync(
        string clientId, string clientSecret, string code, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = RedirectUri,
        };

        return PostTokenRequestAsync(clientId, clientSecret, parameters, cancellationToken);
    }

    private static Task<BlingTokenResponse> RefreshTokenAsync(
        string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken,
        };

        return PostTokenRequestAsync(clientId, clientSecret, parameters, cancellationToken);
    }

    private static async Task<BlingTokenResponse> PostTokenRequestAsync(
        string clientId, string clientSecret, Dictionary<string, string> parameters, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, TokenUrl)
        {
            Content = new FormUrlEncodedContent(parameters),
        };

        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await HttpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new BlingApiException(BuildTokenErrorMessage(response.StatusCode, body));
        }

        var tokenResponse = JsonSerializer.Deserialize<BlingTokenResponse>(body);
        if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new BlingApiException("Resposta inesperada do Bling ao obter o token.");
        }

        return tokenResponse;
    }

    private static string BuildTokenErrorMessage(HttpStatusCode statusCode, string body)
    {
        try
        {
            var parsed = JsonSerializer.Deserialize<BlingOAuthErrorResponse>(body);
            var description = parsed?.Error?.Description ?? parsed?.Error?.Message ?? parsed?.Error?.Type;
            if (!string.IsNullOrWhiteSpace(description))
            {
                return $"Bling retornou erro ({(int)statusCode}): {description}";
            }
        }
        catch (JsonException)
        {
            // corpo não é o formato de erro esperado, cai no retorno bruto abaixo
        }

        return $"Bling retornou erro ({(int)statusCode}): {body}";
    }

    private static string GenerateState() => Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
}
