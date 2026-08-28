namespace BlingOrdensProducao.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private Label lblTitle;
    private Label lblStatusConexao;
    private Button btnTestConnection;
    private Button btnSignOut;

    private Panel pnlSetup;
    private Label lblClientId;
    private TextBox txtClientId;
    private Label lblClientSecret;
    private TextBox txtClientSecret;
    private Button btnAuthorize;

    private Panel pnlLote;
    private Label lblBusca;
    private Button btnAtualizarCatalogo;
    private TextBox txtBusca;
    private ListBox lstSugestoes;
    private Label lblQuantidade;
    private NumericUpDown numQuantidade;
    private Button btnAdicionarItem;
    private ListView lvItens;
    private ColumnHeader colProduto;
    private ColumnHeader colQuantidade;
    private Button btnRemoverItem;
    private Button btnExecutar;

    private TextBox txtLog;

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.lblTitle = new Label();
        this.lblStatusConexao = new Label();
        this.btnTestConnection = new Button();
        this.btnSignOut = new Button();

        this.pnlSetup = new Panel();
        this.lblClientId = new Label();
        this.txtClientId = new TextBox();
        this.lblClientSecret = new Label();
        this.txtClientSecret = new TextBox();
        this.btnAuthorize = new Button();

        this.pnlLote = new Panel();
        this.lblBusca = new Label();
        this.btnAtualizarCatalogo = new Button();
        this.txtBusca = new TextBox();
        this.lstSugestoes = new ListBox();
        this.lblQuantidade = new Label();
        this.numQuantidade = new NumericUpDown();
        this.btnAdicionarItem = new Button();
        this.lvItens = new ListView();
        this.colProduto = new ColumnHeader();
        this.colQuantidade = new ColumnHeader();
        this.btnRemoverItem = new Button();
        this.btnExecutar = new Button();

        this.txtLog = new TextBox();

        ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
        this.pnlSetup.SuspendLayout();
        this.pnlLote.SuspendLayout();
        this.SuspendLayout();

        // lblTitle
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(20, 15);
        this.lblTitle.Text = "Ordens de Produção - Bling";

        // lblStatusConexao
        this.lblStatusConexao.AutoSize = true;
        this.lblStatusConexao.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.lblStatusConexao.Location = new System.Drawing.Point(490, 18);
        this.lblStatusConexao.Text = "Conectado ao Bling";
        this.lblStatusConexao.ForeColor = System.Drawing.Color.DarkGreen;

        // btnTestConnection
        this.btnTestConnection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnTestConnection.Location = new System.Drawing.Point(490, 38);
        this.btnTestConnection.Size = new System.Drawing.Size(100, 26);
        this.btnTestConnection.Text = "Testar conexão";
        this.btnTestConnection.Font = new System.Drawing.Font("Segoe UI", 7.5F);
        this.btnTestConnection.Click += new EventHandler(this.btnTestConnection_Click);

        // btnSignOut
        this.btnSignOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnSignOut.Location = new System.Drawing.Point(600, 38);
        this.btnSignOut.Size = new System.Drawing.Size(100, 26);
        this.btnSignOut.Text = "Desconectar";
        this.btnSignOut.Font = new System.Drawing.Font("Segoe UI", 7.5F);
        this.btnSignOut.Click += new EventHandler(this.btnSignOut_Click);

        // pnlSetup
        this.pnlSetup.Location = new System.Drawing.Point(20, 60);
        this.pnlSetup.Size = new System.Drawing.Size(660, 160);
        this.pnlSetup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.pnlSetup.Controls.Add(this.lblClientId);
        this.pnlSetup.Controls.Add(this.txtClientId);
        this.pnlSetup.Controls.Add(this.lblClientSecret);
        this.pnlSetup.Controls.Add(this.txtClientSecret);
        this.pnlSetup.Controls.Add(this.btnAuthorize);

        // lblClientId
        this.lblClientId.AutoSize = true;
        this.lblClientId.Location = new System.Drawing.Point(0, 0);
        this.lblClientId.Text = "Client ID";

        // txtClientId
        this.txtClientId.Location = new System.Drawing.Point(0, 20);
        this.txtClientId.Size = new System.Drawing.Size(400, 23);
        this.txtClientId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // lblClientSecret
        this.lblClientSecret.AutoSize = true;
        this.lblClientSecret.Location = new System.Drawing.Point(0, 55);
        this.lblClientSecret.Text = "Client Secret";

        // txtClientSecret
        this.txtClientSecret.Location = new System.Drawing.Point(0, 75);
        this.txtClientSecret.Size = new System.Drawing.Size(400, 23);
        this.txtClientSecret.UseSystemPasswordChar = true;
        this.txtClientSecret.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        // btnAuthorize
        this.btnAuthorize.Location = new System.Drawing.Point(0, 115);
        this.btnAuthorize.Size = new System.Drawing.Size(220, 32);
        this.btnAuthorize.Text = "Autorizar acesso ao Bling";
        this.btnAuthorize.Click += new EventHandler(this.btnAuthorize_Click);

        // pnlLote
        this.pnlLote.Location = new System.Drawing.Point(20, 60);
        this.pnlLote.Size = new System.Drawing.Size(660, 330);
        this.pnlLote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.pnlLote.Controls.Add(this.lblBusca);
        this.pnlLote.Controls.Add(this.btnAtualizarCatalogo);
        this.pnlLote.Controls.Add(this.txtBusca);
        this.pnlLote.Controls.Add(this.lstSugestoes);
        this.pnlLote.Controls.Add(this.lblQuantidade);
        this.pnlLote.Controls.Add(this.numQuantidade);
        this.pnlLote.Controls.Add(this.btnAdicionarItem);
        this.pnlLote.Controls.Add(this.lvItens);
        this.pnlLote.Controls.Add(this.btnRemoverItem);
        this.pnlLote.Controls.Add(this.btnExecutar);

        // lblBusca
        this.lblBusca.AutoSize = true;
        this.lblBusca.Location = new System.Drawing.Point(0, 0);
        this.lblBusca.Text = "Produto (digite pra buscar)";

        // btnAtualizarCatalogo
        this.btnAtualizarCatalogo.Location = new System.Drawing.Point(300, -2);
        this.btnAtualizarCatalogo.Size = new System.Drawing.Size(100, 20);
        this.btnAtualizarCatalogo.Font = new System.Drawing.Font("Segoe UI", 7.5F);
        this.btnAtualizarCatalogo.Text = "Atualizar catálogo";
        this.btnAtualizarCatalogo.Click += new EventHandler(this.btnAtualizarCatalogo_Click);

        // txtBusca
        this.txtBusca.Location = new System.Drawing.Point(0, 20);
        this.txtBusca.Size = new System.Drawing.Size(400, 23);
        this.txtBusca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.txtBusca.AutoCompleteMode = AutoCompleteMode.None;
        this.txtBusca.TextChanged += new EventHandler(this.txtBusca_TextChanged);

        // lstSugestoes
        this.lstSugestoes.Location = new System.Drawing.Point(0, 45);
        this.lstSugestoes.Size = new System.Drawing.Size(400, 90);
        this.lstSugestoes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.lstSugestoes.Visible = false;
        this.lstSugestoes.DisplayMember = "ExibicaoAutocomplete";
        this.lstSugestoes.Click += new EventHandler(this.lstSugestoes_Click);

        // lblQuantidade
        this.lblQuantidade.AutoSize = true;
        this.lblQuantidade.Location = new System.Drawing.Point(420, 0);
        this.lblQuantidade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.lblQuantidade.Text = "Quantidade";

        // numQuantidade
        this.numQuantidade.Location = new System.Drawing.Point(420, 20);
        this.numQuantidade.Size = new System.Drawing.Size(80, 23);
        this.numQuantidade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.numQuantidade.Minimum = 1;
        this.numQuantidade.Maximum = 100000;
        this.numQuantidade.Value = 1;

        // btnAdicionarItem
        this.btnAdicionarItem.Location = new System.Drawing.Point(510, 19);
        this.btnAdicionarItem.Size = new System.Drawing.Size(150, 26);
        this.btnAdicionarItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnAdicionarItem.Text = "Adicionar à lista";
        this.btnAdicionarItem.Click += new EventHandler(this.btnAdicionarItem_Click);

        // lvItens
        this.lvItens.Location = new System.Drawing.Point(0, 140);
        this.lvItens.Size = new System.Drawing.Size(660, 150);
        this.lvItens.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.lvItens.View = View.Details;
        this.lvItens.FullRowSelect = true;
        this.lvItens.GridLines = true;
        this.lvItens.MultiSelect = false;
        this.lvItens.Columns.Add(this.colProduto);
        this.lvItens.Columns.Add(this.colQuantidade);

        // colProduto
        this.colProduto.Text = "Produto";
        this.colProduto.Width = 500;

        // colQuantidade
        this.colQuantidade.Text = "Quantidade";
        this.colQuantidade.Width = 120;

        // btnRemoverItem
        this.btnRemoverItem.Location = new System.Drawing.Point(0, 296);
        this.btnRemoverItem.Size = new System.Drawing.Size(160, 28);
        this.btnRemoverItem.Text = "Remover selecionado";
        this.btnRemoverItem.Click += new EventHandler(this.btnRemoverItem_Click);

        // btnExecutar
        this.btnExecutar.Location = new System.Drawing.Point(490, 294);
        this.btnExecutar.Size = new System.Drawing.Size(170, 32);
        this.btnExecutar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnExecutar.Text = "Executar lote";
        this.btnExecutar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnExecutar.Click += new EventHandler(this.btnExecutar_Click);

        // txtLog
        this.txtLog.Location = new System.Drawing.Point(20, 400);
        this.txtLog.Size = new System.Drawing.Size(660, 180);
        this.txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.txtLog.Multiline = true;
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = ScrollBars.Vertical;
        this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);

        // MainForm
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(700, 610);
        this.MinimumSize = new System.Drawing.Size(650, 500);
        this.Text = "Bling - Ordens de Produção";
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblStatusConexao);
        this.Controls.Add(this.btnTestConnection);
        this.Controls.Add(this.btnSignOut);
        this.Controls.Add(this.pnlSetup);
        this.Controls.Add(this.pnlLote);
        this.Controls.Add(this.txtLog);

        ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
        this.pnlSetup.ResumeLayout(false);
        this.pnlSetup.PerformLayout();
        this.pnlLote.ResumeLayout(false);
        this.pnlLote.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
