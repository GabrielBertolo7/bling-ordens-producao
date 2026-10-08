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

    private readonly bool _demo;
    private bool _conectado;

    public MainForm(bool demo = false)
    {
        _demo = demo;
        InitializeComponent();
        AplicarTextos();

        _oAuthService = new BlingOAuthService(_storage);
        _apiClient = new BlingApiClient(_oAuthService);
        _produtoService = new BlingProdutoService(_apiClient);
        _ordemProducaoService = new BlingOrdemProducaoService(_apiClient);

        Resize += (_, _) => PosicionarElementos();
        Shown += (_, _) => dgvItens.ClearSelection();

        if (_demo)
        {
            CarregarDemonstracao();
        }
        else
        {
            RefreshConnectionState();
        }
        AtualizarListaVisual();
    }

    private void AplicarTextos()
    {
        Text = Texts.WindowTitle;
        lblTitle.Text = Texts.Title;
        lblSubtitle.Text = Texts.Subtitle;
        pillStatus.Text = Texts.Connected;
        btnTestConnection.Text = Texts.TestConnection;
        btnSignOut.Text = Texts.SignOut;
        lblSetupTitle.Text = Texts.SetupTitle;
        lblSetupHint.Text = Texts.SetupHint;
        lblClientId.Text = Texts.ClientId;
        lblClientSecret.Text = Texts.ClientSecret;
        btnAuthorize.Text = Texts.Authorize;
        lblLoteTitle.Text = Texts.BatchTitle;
        lblBusca.Text = Texts.ProductLabel;
        btnAtualizarCatalogo.Text = Texts.RefreshCatalog;
        lblQuantidade.Text = Texts.Quantity;
        btnAdicionarItem.Text = Texts.AddItem;
        colProduto.HeaderText = Texts.ColumnProduct;
        colQuantidade.HeaderText = Texts.ColumnQuantity;
        btnRemoverItem.Text = Texts.RemoveItem;
        btnExecutar.Text = Texts.Execute;
        lblLogTitle.Text = Texts.LogTitle;
    }

    // Selo de status alinhado à direita e cartão do registro logo abaixo do cartão visível.
    private void PosicionarElementos()
    {
        pillStatus.Left = ClientSize.Width - 20 - pillStatus.Width;
        // Visible fica falso enquanto a janela não aparece, por isso o estado vem de _conectado.
        var cartaoVisivel = _conectado ? (Control)pnlLote : pnlSetup;
        pnlLog.Top = cartaoVisivel.Bottom + 14;
        pnlLog.Height = Math.Max(140, ClientSize.Height - pnlLog.Top - 20);
    }

    // Modo de demonstração (--demo): tela conectada com dados fictícios, sem chamar a API.
    // Serve só para prints de portfólio.
    private void CarregarDemonstracao()
    {
        MostrarPaineis(conectado: true);
        var itens = Texts.English
            ? new[] { ("Moon lamp 15 cm", 6), ("Small geometric vase", 14), ("Headset stand", 10) }
            : new[] { ("Luminária lua 15 cm", 6), ("Vaso geométrico pequeno", 14), ("Suporte de headset", 10) };
        long id = 1;
        foreach (var (nome, quantidade) in itens)
        {
            _itensLote.Add(new ItemLote { ProdutoId = id++, ProdutoNome = nome, Quantidade = quantidade });
        }

        var anteriores = Texts.English
            ? new[] { ("Articulated phone holder", 12, 9001L), ("Cable organizer", 30, 9002L), ("Dragon keychain", 8, 9003L) }
            : new[] { ("Suporte de celular articulado", 12, 9001L), ("Organizador de cabos", 30, 9002L), ("Porta-chaves dragão", 8, 9003L) };
        Log(Texts.LogLoadingCatalog);
        Log(Texts.LogCatalogLoaded(8));
        Log(Texts.LogStarting(anteriores.Length));
        foreach (var (nome, quantidade, ordem) in anteriores)
        {
            Log(Texts.LogItemOk(nome, quantidade, ordem));
        }
        Log(Texts.LogBatchDone);
    }

    private void MostrarPaineis(bool conectado)
    {
        _conectado = conectado;
        pnlSetup.Visible = !conectado;
        pnlLote.Visible = conectado;
        pillStatus.Visible = conectado;
        btnTestConnection.Visible = conectado;
        btnSignOut.Visible = conectado;
        PosicionarElementos();
    }

    private void RefreshConnectionState()
    {
        var isAuthorized = _oAuthService.IsAuthorized;
        MostrarPaineis(isAuthorized);

        if (isAuthorized && !_produtoService.CatalogoCarregado && !_carregandoCatalogo)
        {
            _ = CarregarCatalogoAsync();
        }
    }

    private async Task CarregarCatalogoAsync()
    {
        _carregandoCatalogo = true;
        txtBusca.Enabled = false;
        lblBusca.Text = Texts.LoadingCatalog;
        Log(Texts.LogLoadingCatalog);

        try
        {
            await _produtoService.CarregarCatalogoAsync();
            lblBusca.Text = Texts.ProductLabel;
            Log(Texts.LogCatalogLoaded(_produtoService.TotalProdutosCarregados));
        }
        catch (Exception ex)
        {
            lblBusca.Text = Texts.CatalogFailed;
            Log(Texts.LogCatalogError(ex.Message));
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
            MessageBox.Show(this, Texts.IncompleteMessage, Texts.IncompleteTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnAuthorize.Enabled = false;
        Log(Texts.LogOpeningBrowser);

        try
        {
            await _oAuthService.AuthorizeAsync(clientId, clientSecret);
            Log(Texts.LogAuthorized);
            RefreshConnectionState();
        }
        catch (Exception ex)
        {
            Log(Texts.LogAuthFailed(ex.Message));
            MessageBox.Show(this, ex.Message, Texts.AuthFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnAuthorize.Enabled = true;
        }
    }

    private async void btnTestConnection_Click(object? sender, EventArgs e)
    {
        btnTestConnection.Enabled = false;
        Log(Texts.LogTesting);

        try
        {
            await _apiClient.TestConnectionAsync();
            Log(Texts.LogConnectionOk);
        }
        catch (Exception ex)
        {
            Log(Texts.LogConnectionFailed(ex.Message));
        }
        finally
        {
            btnTestConnection.Enabled = true;
        }
    }

    private void btnSignOut_Click(object? sender, EventArgs e)
    {
        var confirm = MessageBox.Show(this, Texts.SignOutConfirm, Texts.SignOut, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        _oAuthService.SignOut();
        Log(Texts.LogSignedOut);
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
            MessageBox.Show(this, Texts.NoProductMessage, Texts.NoProductTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        if (dgvItens.CurrentRow is null || dgvItens.CurrentRow.Index >= _itensLote.Count)
        {
            return;
        }

        _itensLote.RemoveAt(dgvItens.CurrentRow.Index);
        AtualizarListaVisual();
    }

    private void AtualizarListaVisual()
    {
        dgvItens.Rows.Clear();
        foreach (var item in _itensLote)
        {
            dgvItens.Rows.Add(item.ProdutoNome, item.Quantidade.ToString());
        }
        dgvItens.ClearSelection();
        lblBatchSummary.Text = _itensLote.Count == 0
            ? string.Empty
            : Texts.BatchSummary(_itensLote.Count, _itensLote.Sum(i => i.Quantidade));
    }

    // ---------- Execução do lote ----------

    private async void btnExecutar_Click(object? sender, EventArgs e)
    {
        if (_itensLote.Count == 0)
        {
            MessageBox.Show(this, Texts.EmptyMessage, Texts.EmptyTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_demo)
        {
            return; // modo de demonstração nunca chama a API
        }

        var confirm = MessageBox.Show(this, Texts.ConfirmMessage(_itensLote.Count), Texts.ConfirmTitle,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        btnExecutar.Enabled = false;
        btnAdicionarItem.Enabled = false;

        Log(Texts.LogStarting(_itensLote.Count));

        foreach (var item in _itensLote.ToList())
        {
            try
            {
                var idOrdem = await _ordemProducaoService.CriarEFinalizarAsync(item);
                Log(Texts.LogItemOk(item.ProdutoNome, item.Quantidade, idOrdem));
                _itensLote.Remove(item);
            }
            catch (Exception ex)
            {
                Log(Texts.LogItemError(item.ProdutoNome, item.Quantidade, ex.Message));
            }
        }

        AtualizarListaVisual();
        Log(_itensLote.Count == 0 ? Texts.LogBatchDone : Texts.LogBatchPending(_itensLote.Count));

        btnExecutar.Enabled = true;
        btnAdicionarItem.Enabled = true;
    }
}
