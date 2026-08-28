namespace BlingOrdensProducao.Services;

// Erro ao chamar a API do Bling, seja no fluxo de autenticação ou em qualquer outro endpoint.
public sealed class BlingApiException : Exception
{
    public BlingApiException(string message) : base(message)
    {
    }

    public BlingApiException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
