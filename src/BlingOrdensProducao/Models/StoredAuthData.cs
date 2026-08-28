namespace BlingOrdensProducao.Models;

/// <summary>
/// Tudo que é persistido localmente (criptografado via DPAPI) para autenticar com o Bling
/// sem exigir que o usuário refaça o login a cada execução.
/// </summary>
public sealed class StoredAuthData
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>Instante (UTC) em que o access_token expira, calculado a partir do expires_in retornado pelo Bling.</summary>
    public DateTime AccessTokenExpiresAtUtc { get; set; }
}
