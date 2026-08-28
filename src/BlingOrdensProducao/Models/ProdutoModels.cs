using System.Text.Json.Serialization;

namespace BlingOrdensProducao.Models;

public sealed class ProdutoResumo
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("codigo")]
    public string? Codigo { get; set; }

    [JsonPropertyName("estoque")]
    public EstoqueResumo? Estoque { get; set; }

    // Texto mostrado na lista de sugestões do autocomplete: nome, código e estoque atual, pra
    // ajudar a confirmar visualmente que é o produto certo antes de escolher.
    public string ExibicaoAutocomplete
    {
        get
        {
            var partes = new List<string> { Nome };
            if (!string.IsNullOrWhiteSpace(Codigo))
            {
                partes.Add($"[{Codigo}]");
            }
            if (Estoque is not null)
            {
                partes.Add($"(estoque: {Estoque.SaldoVirtualTotal:0.##})");
            }
            return string.Join(" ", partes);
        }
    }

    public override string ToString() => ExibicaoAutocomplete;
}

public sealed class EstoqueResumo
{
    [JsonPropertyName("saldoVirtualTotal")]
    public decimal SaldoVirtualTotal { get; set; }
}
