using System.Text.Json.Serialization;

namespace BlingOrdensProducao.Models;

public sealed class BlingTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonPropertyName("expires_in")]
    public int ExpiresInSeconds { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;
}

public sealed class BlingOAuthErrorResponse
{
    [JsonPropertyName("error")]
    public BlingOAuthError? Error { get; set; }
}

public sealed class BlingOAuthError
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
