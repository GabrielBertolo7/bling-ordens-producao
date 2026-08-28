using System.Text.Json;
using BlingOrdensProducao.Models;

namespace BlingOrdensProducao.Services;

// Cria e finaliza Ordens de Produção no Bling. Nenhum id (módulo, situação, depósito) fica fixo
// no código: tudo é descoberto em runtime, porque varia por conta e pode mudar no painel.
public sealed class BlingOrdemProducaoService
{
    private const string ModuloOrdensProducaoNomeSistema = "OrdensProducao";
    private const string SituacaoInicialNome = "Não iniciado";
    private const string SituacaoFinalNome = "Finalizado";

    private readonly BlingApiClient _apiClient;
    private long? _idModuloCache;
    private long? _idDepositoCache;

    public BlingOrdemProducaoService(BlingApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Cria a ordem na situação inicial e já finaliza em seguida, que é o que credita o produto
    // no estoque. Representa um item de lote de uma das impressoras.
    public async Task<long> CriarEFinalizarAsync(ItemLote item, CancellationToken cancellationToken = default)
    {
        var idSituacaoInicial = await ObterIdSituacaoAsync(SituacaoInicialNome, cancellationToken);
        var idDeposito = await ObterIdDepositoPadraoAsync(cancellationToken);
        var numero = await ObterProximoNumeroAsync(cancellationToken);

        var payloadCriacao = new
        {
            numero,
            deposito = new { idOrigem = idDeposito, idDestino = idDeposito },
            situacao = new { id = idSituacaoInicial },
            itens = new[]
            {
                new { produto = new { id = item.ProdutoId }, quantidade = item.Quantidade },
            },
        };

        var respostaCriacao = await _apiClient.PostRawAsync("/ordens-producao", payloadCriacao, cancellationToken);
        var idOrdem = ParseCreatedId(respostaCriacao);

        var idSituacaoFinal = await ObterIdSituacaoAsync(SituacaoFinalNome, cancellationToken);
        var payloadFinalizacao = new { idSituacao = idSituacaoFinal, quantidade = item.Quantidade };

        // O verbo correto aqui é PUT. A documentação da API e os SDKs de terceiros indicam PATCH,
        // mas essa rota responde 404 com PATCH e funciona normalmente com PUT.
        await _apiClient.PutRawAsync($"/ordens-producao/{idOrdem}/situacoes", payloadFinalizacao, cancellationToken);

        return idOrdem;
    }

    // Maior número de ordem já usado na conta, mais um. Consultado a cada criação, sem cache, pra
    // não colidir com uma ordem criada manualmente entre uma execução e outra do app.
    public async Task<int> ObterProximoNumeroAsync(CancellationToken cancellationToken = default)
    {
        var raw = await _apiClient.GetRawAsync("/ordens-producao?limite=10&pagina=1", cancellationToken);
        var ordens = ParseData<OrdemProducaoResumo>(raw);
        var maiorNumero = ordens.Count > 0 ? ordens.Max(o => o.Numero) : 0;
        return maiorNumero + 1;
    }

    private async Task<long> ObterIdModuloAsync(CancellationToken cancellationToken)
    {
        if (_idModuloCache is { } cached)
        {
            return cached;
        }

        var raw = await _apiClient.GetRawAsync("/situacoes/modulos", cancellationToken);
        var modulos = ParseData<Modulo>(raw);
        var modulo = modulos.FirstOrDefault(m => m.Nome == ModuloOrdensProducaoNomeSistema)
            ?? throw new BlingApiException($"Módulo '{ModuloOrdensProducaoNomeSistema}' não encontrado nessa conta Bling.");

        _idModuloCache = modulo.Id;
        return modulo.Id;
    }

    // Depósito marcado como padrão na conta, sem id fixo no código.
    private async Task<long> ObterIdDepositoPadraoAsync(CancellationToken cancellationToken)
    {
        if (_idDepositoCache is { } cached)
        {
            return cached;
        }

        var raw = await _apiClient.GetRawAsync("/depositos", cancellationToken);
        var depositos = ParseData<Deposito>(raw);
        var deposito = depositos.FirstOrDefault(d => d.Padrao)
            ?? throw new BlingApiException("Nenhum depósito padrão encontrado nessa conta Bling.");

        _idDepositoCache = deposito.Id;
        return deposito.Id;
    }

    private async Task<long> ObterIdSituacaoAsync(string nomeSituacao, CancellationToken cancellationToken)
    {
        var idModulo = await ObterIdModuloAsync(cancellationToken);
        var raw = await _apiClient.GetRawAsync($"/situacoes/modulos/{idModulo}", cancellationToken);
        var situacoes = ParseData<SituacaoModulo>(raw);

        var situacao = situacoes.FirstOrDefault(s => string.Equals(s.Nome, nomeSituacao, StringComparison.OrdinalIgnoreCase));
        if (situacao is null)
        {
            var disponiveis = string.Join(", ", situacoes.Select(s => s.Nome));
            throw new BlingApiException(
                $"Situação '{nomeSituacao}' não encontrada no módulo de Ordens de Produção. Disponíveis: {disponiveis}");
        }

        return situacao.Id;
    }

    private static List<T> ParseData<T>(string json)
    {
        using var document = JsonDocument.Parse(json);
        if (!document.RootElement.TryGetProperty("data", out var dataElement))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<T>>(dataElement.GetRawText()) ?? [];
    }

    private static long ParseCreatedId(string json)
    {
        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("data").GetProperty("id").GetInt64();
    }
}
