using System.Text.Json;
using BlingOrdensProducao.Models;

namespace BlingOrdensProducao.Services;

// Catálogo de produtos do Bling, usado para alimentar o autocomplete da tela principal.
// A API não tem uma busca que "contém em qualquer lugar do nome": só prefixo de nome ou código
// exato, inviável pra autocomplete. Por isso o catálogo inteiro é carregado uma vez (é pequeno,
// ~280 produtos) e a busca roda localmente, em memória.
public sealed class BlingProdutoService
{
    private const int TamanhoPagina = 100;
    private const int LimitePaginasSeguranca = 50;

    private readonly BlingApiClient _apiClient;
    private List<ProdutoResumo> _catalogo = [];

    public BlingProdutoService(BlingApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public bool CatalogoCarregado { get; private set; }

    public int TotalProdutosCarregados => _catalogo.Count;

    public async Task CarregarCatalogoAsync(CancellationToken cancellationToken = default)
    {
        var todos = new List<ProdutoResumo>();

        for (var pagina = 1; pagina <= LimitePaginasSeguranca; pagina++)
        {
            var raw = await _apiClient.GetRawAsync($"/produtos?pagina={pagina}&limite={TamanhoPagina}", cancellationToken);
            var itens = ParseData(raw);
            if (itens.Count == 0)
            {
                break;
            }

            todos.AddRange(itens);

            if (itens.Count < TamanhoPagina)
            {
                break; // última página
            }
        }

        _catalogo = todos;
        CatalogoCarregado = true;
    }

    // Busca local (case-insensitive) por nome ou código, no catálogo já carregado em memória.
    public List<ProdutoResumo> Buscar(string texto, int maxResultados = 20)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return [];
        }

        return _catalogo
            .Where(p => p.Nome.Contains(texto, StringComparison.OrdinalIgnoreCase)
                     || (!string.IsNullOrEmpty(p.Codigo) && p.Codigo.Contains(texto, StringComparison.OrdinalIgnoreCase)))
            .Take(maxResultados)
            .ToList();
    }

    private static List<ProdutoResumo> ParseData(string json)
    {
        using var document = JsonDocument.Parse(json);
        if (!document.RootElement.TryGetProperty("data", out var dataElement))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<ProdutoResumo>>(dataElement.GetRawText()) ?? [];
    }
}
