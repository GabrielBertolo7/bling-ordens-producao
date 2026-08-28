using BlingOrdensProducao.Models;
using BlingOrdensProducao.Services;

namespace BlingOrdensProducao.Forms;

public partial class MainForm : Form
{
    private readonly SecureTokenStorage _storage = new();
    private readonly BlingOAuthService _oAuthService;
    private readonly BlingApiClient _apiClient;
    private readonly BlingProdutoService _produtoService;
    private readonly BlingOrdemProducaoService _ordemProducaoService;

    private readonly List<ItemLote> _itensLote = [];
    private long? _produtoSelecionadoId;
    private string _produtoSelecionadoNome = string.Empty;
    private bool _suprimirBuscaTextChanged;
    private bool _carregandoCatalogo;

    public MainForm()
    {
        InitializeComponent();

        _oAuthService = new BlingOAuthService(_storage);
        _apiClient = new BlingApiClient(_oAuthService);
        _produtoService = new BlingProdutoService(_apiClient);
        _ordemProducaoService = new BlingOrdemProducaoService(_apiClient);

        RefreshConnectionState();
    }

    private void RefreshConnectionState()
    {
        var isAuthorized = _oAuthService.IsAuthorized;

        pnlSetup.Visible = !isAuthorized;
        pnlLote.Visible = isAuthorized;
        lblStatusConexao.Visible = isAuthorized;
        btnTestConnection.Visible = isAuthorized;
        btnSignOut.Visible = isAuthorized;

        if (isAuthorized && !_produtoService.CatalogoCarregado && !_carregandoCatalogo)
        {
            _ = CarregarCatalogoAsync();
        }
    }

    private async Task CarregarCatalogoAsync()
    {
        _carregandoCatalogo = true;
        txtBusca.Enabled = false;
        lblBusca.Text = "Carregando catálogo de produtos...";
        Log("Carregando catálogo de produtos do Bling...");

        try
        {
            await _produtoService.CarregarCatalogoAsync();
            lblBusca.Text = "Produto (digite pra buscar)";
            Log($"Catálogo carregado: {_produtoService.TotalProdutosCarregados} produtos.");
        }
        catch (Exception ex)
        {
            lblBusca.Text = "Produto (falha ao carregar catálogo, clique em \"Atualizar catálogo\")";
            Log($"Erro ao carregar catálogo de produtos: {ex.Message}");
        }
        finally
        {
            txtBusca.Enabled = true;
            _carregandoCatalogo = false;
        }
    }

    private void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        txtLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
    }

    // ---------- Autorização ----------

    private async void btnAuthorize_Click(object? sender, EventArgs e)
    {
        var clientId = txtClientId.Text.Trim();
        var clientSecret = txtClientSecret.Text.Trim();

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            MessageBox.Show(this, "Informe o Client ID e o Client Secret do aplicativo cadastrado no Bling.",
                "Dados incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnAuthorize.Enabled = false;
        Log("Abrindo o navegador para autorização no Bling...");

        try
        {
            await _oAuthService.AuthorizeAsync(clientId, clientSecret);
            Log("Autorização concluída com sucesso. Tokens salvos localmente.");
            RefreshConnectionState();
        }
        catch (Exception ex)
        {
            Log($"Falha na autorização: {ex.Message}");
            MessageBox.Show(this, ex.Message, "Falha na autorização", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnAuthorize.Enabled = true;
        }
    }

    private async void btnTestConnection_Click(object? sender, EventArgs e)
    {
        btnTestConnection.Enabled = false;
        Log("Testando conexão com a API do Bling...");

        try
        {
            await _apiClient.TestConnectionAsync();
            Log("Conexão OK: a API do Bling respondeu normalmente.");
        }
        catch (Exception ex)
        {
            Log($"Falha ao testar conexão: {ex.Message}");
        }
        finally
        {
            btnTestConnection.Enabled = true;
        }
    }

    private void btnSignOut_Click(object? sender, EventArgs e)
    {
        var confirm = MessageBox.Show(this,
            "Isso vai apagar o token salvo localmente. Será necessário autorizar novamente. Continuar?",
            "Desconectar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        _oAuthService.SignOut();
        Log("Desconectado. Token local removido.");
        RefreshConnectionState();
    }

    // ---------- Busca de produto (autocomplete local, catálogo já carregado em memória) ----------

    private void txtBusca_TextChanged(object? sender, EventArgs e)
    {
        if (_suprimirBuscaTextChanged)
        {
            return;
        }

        _produtoSelecionadoId = null;
        _produtoSelecionadoNome = string.Empty;

        var texto = txtBusca.Text.Trim();
        if (texto.Length < 2)
        {
            lstSugestoes.Items.Clear();
            lstSugestoes.Visible = false;
            return;
        }

        var resultados = _produtoService.Buscar(texto);

        lstSugestoes.Items.Clear();
        foreach (var produto in resultados)
        {
            lstSugestoes.Items.Add(produto);
        }
        lstSugestoes.Visible = resultados.Count > 0;
    }

    private async void btnAtualizarCatalogo_Click(object? sender, EventArgs e)
    {
        if (_carregandoCatalogo)
        {
            return;
        }

        await CarregarCatalogoAsync();
    }

    private void lstSugestoes_Click(object? sender, EventArgs e)
    {
        if (lstSugestoes.SelectedItem is not ProdutoResumo produto)
        {
            return;
        }

        _produtoSelecionadoId = produto.Id;
        _produtoSelecionadoNome = produto.Nome;

        _suprimirBuscaTextChanged = true;
        txtBusca.Text = produto.Nome;
        _suprimirBuscaTextChanged = false;

        lstSugestoes.Items.Clear();
        lstSugestoes.Visible = false;

        numQuantidade.Focus();
        numQuantidade.Select(0, numQuantidade.Text.Length);
    }

    // ---------- Lista de lote ----------

    private void btnAdicionarItem_Click(object? sender, EventArgs e)
    {
        if (_produtoSelecionadoId is null)
        {
            MessageBox.Show(this, "Busque e selecione um produto da lista de sugestões antes de adicionar.",
                "Nenhum produto selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var quantidade = (int)numQuantidade.Value;

        _itensLote.Add(new ItemLote
        {
            ProdutoId = _produtoSelecionadoId.Value,
            ProdutoNome = _produtoSelecionadoNome,
            Quantidade = quantidade,
        });

        AtualizarListaVisual();

        _produtoSelecionadoId = null;
        _produtoSelecionadoNome = string.Empty;
        _suprimirBuscaTextChanged = true;
        txtBusca.Clear();
        _suprimirBuscaTextChanged = false;
        numQuantidade.Value = 1;
        txtBusca.Focus();
    }

    private void btnRemoverItem_Click(object? sender, EventArgs e)
    {
        if (lvItens.SelectedIndices.Count == 0)
        {
            return;
        }

        _itensLote.RemoveAt(lvItens.SelectedIndices[0]);
        AtualizarListaVisual();
    }

    private void AtualizarListaVisual()
    {
        lvItens.Items.Clear();
        foreach (var item in _itensLote)
        {
            var linha = new ListViewItem(item.ProdutoNome);
            linha.SubItems.Add(item.Quantidade.ToString());
            lvItens.Items.Add(linha);
        }
    }

    // ---------- Execução do lote ----------

    private async void btnExecutar_Click(object? sender, EventArgs e)
    {
        if (_itensLote.Count == 0)
        {
            MessageBox.Show(this, "Adicione ao menos um item à lista antes de executar.",
                "Lista vazia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(this,
            $"Isso vai criar e finalizar {_itensLote.Count} ordem(ns) de produção no Bling agora, " +
            "creditando o estoque de cada produto. Continuar?",
            "Confirmar execução", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        btnExecutar.Enabled = false;
        btnAdicionarItem.Enabled = false;

        Log($"Iniciando execução de {_itensLote.Count} item(ns)...");

        foreach (var item in _itensLote.ToList())
        {
            try
            {
                var idOrdem = await _ordemProducaoService.CriarEFinalizarAsync(item);
                Log($"OK: {item.ProdutoNome} x{item.Quantidade}, ordem de produção #{idOrdem} criada e finalizada.");
                _itensLote.Remove(item);
            }
            catch (Exception ex)
            {
                Log($"ERRO ({item.ProdutoNome} x{item.Quantidade}): {ex.Message}");
            }
        }

        AtualizarListaVisual();
        Log(_itensLote.Count == 0
            ? "Lote concluído: todos os itens foram processados com sucesso."
            : $"Lote processado com {_itensLote.Count} item(ns) pendente(s) (falharam). Corrija e tente executar de novo.");

        btnExecutar.Enabled = true;
        btnAdicionarItem.Enabled = true;
    }
}
