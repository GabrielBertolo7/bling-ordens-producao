using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlingOrdensProducao.Services;

// Cliente HTTP fino para a API v3 do Bling (endpoints de recurso, não o fluxo de OAuth em si).
public sealed class BlingApiClient
{
    private const string BaseUrl = "https://api.bling.com.br/Api/v3";

    private static readonly HttpClient HttpClient = new();

    private readonly BlingOAuthService _oAuthService;

    public BlingApiClient(BlingOAuthService oAuthService)
    {
        _oAuthService = oAuthService;
    }

    // Chamada barata (1 produto) só pra confirmar que o token autentica de verdade.
    public Task<string> TestConnectionAsync(CancellationToken cancellationToken = default) =>
        GetRawAsync("/produtos?limite=1", cancellationToken);

    public Task<string> GetRawAsync(string relativePathAndQuery, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Get, relativePathAndQuery, content: null, cancellationToken);

    public Task<string> PostRawAsync(string relativePathAndQuery, object payload, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Post, relativePathAndQuery, JsonContent.Create(payload), cancellationToken);

    public Task<string> PatchRawAsync(string relativePathAndQuery, object payload, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Patch, relativePathAndQuery, JsonContent.Create(payload), cancellationToken);

    public Task<string> PutRawAsync(string relativePathAndQuery, object payload, CancellationToken cancellationToken = default) =>
        SendAsync(HttpMethod.Put, relativePathAndQuery, JsonContent.Create(payload), cancellationToken);

    private async Task<string> SendAsync(HttpMethod method, string relativePathAndQuery, HttpContent? content, CancellationToken cancellationToken)
    {
        var accessToken = await _oAuthService.GetValidAccessTokenAsync(cancellationToken);

        using var request = new HttpRequestMessage(method, $"{BaseUrl}{relativePathAndQuery}") { Content = content };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var response = await HttpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new BlingApiException($"Bling retornou erro ({(int)response.StatusCode}) em {method} {relativePathAndQuery}: {body}");
        }

        return body;
    }
}
