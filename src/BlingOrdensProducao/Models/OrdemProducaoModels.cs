using System.Text.Json.Serialization;

namespace BlingOrdensProducao.Models;

public sealed class Modulo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;
}

public sealed class SituacaoModulo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
}

public sealed class OrdemProducaoResumo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("numero")]
    public int Numero { get; set; }
}

public sealed class Deposito
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [JsonPropertyName("padrao")]
    public bool Padrao { get; set; }
}

// Item de lote a ser lançado: produto + quantidade produzida por uma das impressoras.
public sealed class ItemLote
{
    public long ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
