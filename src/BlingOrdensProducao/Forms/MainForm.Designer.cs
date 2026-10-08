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
    private Label lblSubtitle;
    private StatusPill pillStatus;
    private Button btnTestConnection;
    private Button btnSignOut;

    private CardPanel pnlSetup;
    private Label lblSetupTitle;
    private Label lblSetupHint;
    private Label lblClientId;
    private TextBox txtClientId;
    private Label lblClientSecret;
    private TextBox txtClientSecret;
    private Button btnAuthorize;

    private CardPanel pnlLote;
    private Label lblLoteTitle;
    private Label lblBatchSummary;
    private Label lblBusca;
    private Button btnAtualizarCatalogo;
    private TextBox txtBusca;
    private ListBox lstSugestoes;
    private Label lblQuantidade;
    private NumericUpDown numQuantidade;
    private Button btnAdicionarItem;
    private DataGridView dgvItens;
    private DataGridViewTextBoxColumn colProduto;
    private DataGridViewTextBoxColumn colQuantidade;
    private Button btnRemoverItem;
    private Button btnExecutar;

    private CardPanel pnlLog;
    private Label lblLogTitle;
    private Panel pnlLogBox;
    private TextBox txtLog;

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.lblTitle = new Label();
        this.lblSubtitle = new Label();
        this.pillStatus = new StatusPill();
        this.btnTestConnection = new Button();
        this.btnSignOut = new Button();

        this.pnlSetup = new CardPanel();
        this.lblSetupTitle = new Label();
        this.lblSetupHint = new Label();
        this.lblClientId = new Label();
        this.txtClientId = new TextBox();
        this.lblClientSecret = new Label();
        this.txtClientSecret = new TextBox();
        this.btnAuthorize = new Button();

        this.pnlLote = new CardPanel();
        this.lblLoteTitle = new Label();
        this.lblBatchSummary = new Label();
        this.lblBusca = new Label();
        this.btnAtualizarCatalogo = new Button();
        this.txtBusca = new TextBox();
        this.lstSugestoes = new ListBox();
        this.lblQuantidade = new Label();
        this.numQuantidade = new NumericUpDown();
        this.btnAdicionarItem = new Button();
        this.dgvItens = new DataGridView();
        this.colProduto = new DataGridViewTextBoxColumn();
        this.colQuantidade = new DataGridViewTextBoxColumn();
        this.btnRemoverItem = new Button();
        this.btnExecutar = new Button();

        this.pnlLog = new CardPanel();
        this.lblLogTitle = new Label();
        this.pnlLogBox = new Panel();
        this.txtLog = new TextBox();

        ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
        this.pnlSetup.SuspendLayout();
        this.pnlLote.SuspendLayout();
        this.pnlLog.SuspendLayout();
        this.pnlLogBox.SuspendLayout();
        this.SuspendLayout();

        // ---------- Cabeçalho ----------

        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = Theme.Title;
        this.lblTitle.ForeColor = Theme.Text;
        this.lblTitle.Location = new System.Drawing.Point(18, 16);

        this.lblSubtitle.AutoSize = true;
        this.lblSubtitle.Font = Theme.Subtitle;
        this.lblSubtitle.ForeColor = Theme.Muted;
        this.lblSubtitle.Location = new System.Drawing.Point(21, 52);

        this.pillStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.pillStatus.Location = new System.Drawing.Point(590, 18);

        this.btnTestConnection.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnTestConnection.Size = new System.Drawing.Size(132, 32);
        this.btnTestConnection.Location = new System.Drawing.Point(488, 52);
        Theme.SecondaryButton(this.btnTestConnection);
        this.btnTestConnection.Click += new EventHandler(this.btnTestConnection_Click);

        this.btnSignOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.btnSignOut.Size = new System.Drawing.Size(112, 32);
        this.btnSignOut.Location = new System.Drawing.Point(628, 52);
        Theme.SecondaryButton(this.btnSignOut);
        this.btnSignOut.Click += new EventHandler(this.btnSignOut_Click);

        // ---------- Cartão de autorização (antes de conectar) ----------

        this.pnlSetup.Location = new System.Drawing.Point(20, 100);
        this.pnlSetup.Size = new System.Drawing.Size(720, 252);
        this.pnlSetup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.pnlSetup.Controls.Add(this.lblSetupTitle);
        this.pnlSetup.Controls.Add(this.lblSetupHint);
        this.pnlSetup.Controls.Add(this.lblClientId);
        this.pnlSetup.Controls.Add(this.txtClientId);
        this.pnlSetup.Controls.Add(this.lblClientSecret);
        this.pnlSetup.Controls.Add(this.txtClientSecret);
        this.pnlSetup.Controls.Add(this.btnAuthorize);

        this.lblSetupTitle.AutoSize = true;
        this.lblSetupTitle.Font = Theme.CardTitle;
        this.lblSetupTitle.ForeColor = Theme.Text;
        this.lblSetupTitle.Location = new System.Drawing.Point(18, 14);

        this.lblSetupHint.AutoSize = true;
        this.lblSetupHint.ForeColor = Theme.Muted;
        this.lblSetupHint.Location = new System.Drawing.Point(18, 40);

        this.lblClientId.AutoSize = true;
        this.lblClientId.Font = Theme.Label;
        this.lblClientId.ForeColor = Theme.Muted;
        this.lblClientId.Location = new System.Drawing.Point(18, 72);

        this.txtClientId.Font = Theme.Input;
        this.txtClientId.BorderStyle = BorderStyle.FixedSingle;
        this.txtClientId.Location = new System.Drawing.Point(18, 92);
        this.txtClientId.Size = new System.Drawing.Size(420, 26);

        this.lblClientSecret.AutoSize = true;
        this.lblClientSecret.Font = Theme.Label;
        this.lblClientSecret.ForeColor = Theme.Muted;
        this.lblClientSecret.Location = new System.Drawing.Point(18, 130);

        this.txtClientSecret.Font = Theme.Input;
        this.txtClientSecret.BorderStyle = BorderStyle.FixedSingle;
        this.txtClientSecret.Location = new System.Drawing.Point(18, 150);
        this.txtClientSecret.Size = new System.Drawing.Size(420, 26);
        this.txtClientSecret.UseSystemPasswordChar = true;

        this.btnAuthorize.Location = new System.Drawing.Point(18, 198);
        this.btnAuthorize.Size = new System.Drawing.Size(240, 38);
        Theme.PrimaryButton(this.btnAuthorize);
        this.btnAuthorize.Click += new EventHandler(this.btnAuthorize_Click);

        // ---------- Cartão do lote ----------

        this.pnlLote.Location = new System.Drawing.Point(20, 100);
        this.pnlLote.Size = new System.Drawing.Size(720, 366);
        this.pnlLote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.pnlLote.Controls.Add(this.lstSugestoes);
        this.pnlLote.Controls.Add(this.lblLoteTitle);
        this.pnlLote.Controls.Add(this.lblBatchSummary);
        this.pnlLote.Controls.Add(this.lblBusca);
        this.pnlLote.Controls.Add(this.btnAtualizarCatalogo);
        this.pnlLote.Controls.Add(this.txtBusca);
        this.pnlLote.Controls.Add(this.lblQuantidade);
        this.pnlLote.Controls.Add(this.numQuantidade);
        this.pnlLote.Controls.Add(this.btnAdicionarItem);
        this.pnlLote.Controls.Add(this.dgvItens);
        this.pnlLote.Controls.Add(this.btnRemoverItem);
        this.pnlLote.Controls.Add(this.btnExecutar);

        this.lblLoteTitle.AutoSize = true;
        this.lblLoteTitle.Font = Theme.CardTitle;
        this.lblLoteTitle.ForeColor = Theme.Text;
        this.lblLoteTitle.Location = new System.Drawing.Point(18, 14);

        this.lblBatchSummary.AutoSize = false;
        this.lblBatchSummary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.lblBatchSummary.ForeColor = Theme.Muted;
        this.lblBatchSummary.TextAlign = ContentAlignment.MiddleRight;
        this.lblBatchSummary.Location = new System.Drawing.Point(452, 14);
        this.lblBatchSummary.Size = new System.Drawing.Size(250, 24);

        this.lblBusca.AutoSize = true;
        this.lblBusca.Font = Theme.Label;
        this.lblBusca.ForeColor = Theme.Muted;
        this.lblBusca.Location = new System.Drawing.Point(18, 50);

        this.btnAtualizarCatalogo.FlatStyle = FlatStyle.Flat;
        this.btnAtualizarCatalogo.FlatAppearance.BorderSize = 0;
        this.btnAtualizarCatalogo.FlatAppearance.MouseOverBackColor = Theme.PrimarySoft;
        this.btnAtualizarCatalogo.BackColor = Theme.Surface;
        this.btnAtualizarCatalogo.ForeColor = Theme.Primary;
        this.btnAtualizarCatalogo.Font = Theme.Label;
        this.btnAtualizarCatalogo.Cursor = Cursors.Hand;
        this.btnAtualizarCatalogo.TextAlign = ContentAlignment.MiddleRight;
        this.btnAtualizarCatalogo.Location = new System.Drawing.Point(300, 45);
        this.btnAtualizarCatalogo.Size = new System.Drawing.Size(150, 24);
        this.btnAtualizarCatalogo.Click += new EventHandler(this.btnAtualizarCatalogo_Click);

        this.txtBusca.Font = Theme.Input;
        this.txtBusca.BorderStyle = BorderStyle.FixedSingle;
        this.txtBusca.Location = new System.Drawing.Point(18, 72);
        this.txtBusca.Size = new System.Drawing.Size(432, 26);
        this.txtBusca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.txtBusca.AutoCompleteMode = AutoCompleteMode.None;
        this.txtBusca.TextChanged += new EventHandler(this.txtBusca_TextChanged);

        this.lstSugestoes.Font = Theme.Base;
        this.lstSugestoes.BorderStyle = BorderStyle.FixedSingle;
        this.lstSugestoes.Location = new System.Drawing.Point(18, 100);
        this.lstSugestoes.Size = new System.Drawing.Size(432, 120);
        this.lstSugestoes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        this.lstSugestoes.Visible = false;
        this.lstSugestoes.DisplayMember = "ExibicaoAutocomplete";
        this.lstSugestoes.Click += new EventHandler(this.lstSugestoes_Click);

        this.lblQuantidade.AutoSize = true;
        this.lblQuantidade.Font = Theme.Label;
        this.lblQuantidade.ForeColor = Theme.Muted;
        this.lblQuantidade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.lblQuantidade.Location = new System.Drawing.Point(466, 50);

        this.numQuantidade.Font = Theme.Input;
        this.numQuantidade.BorderStyle = BorderStyle.FixedSingle;
        this.numQuantidade.Location = new System.Drawing.Point(466, 72);
        this.numQuantidade.Size = new System.Drawing.Size(90, 26);
        this.numQuantidade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.numQuantidade.Minimum = 1;
        this.numQuantidade.Maximum = 100000;
        this.numQuantidade.Value = 1;

        this.btnAdicionarItem.Location = new System.Drawing.Point(566, 70);
        this.btnAdicionarItem.Size = new System.Drawing.Size(136, 32);
        this.btnAdicionarItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        Theme.SecondaryButton(this.btnAdicionarItem);
        this.btnAdicionarItem.ForeColor = Theme.Primary;
        this.btnAdicionarItem.FlatAppearance.BorderColor = Theme.Primary;
        this.btnAdicionarItem.FlatAppearance.MouseOverBackColor = Theme.PrimarySoft;
        this.btnAdicionarItem.Click += new EventHandler(this.btnAdicionarItem_Click);

        this.dgvItens.Location = new System.Drawing.Point(18, 116);
        this.dgvItens.Size = new System.Drawing.Size(684, 182);
        this.dgvItens.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        Theme.Grid(this.dgvItens);
        this.dgvItens.Columns.AddRange(new DataGridViewColumn[] { this.colProduto, this.colQuantidade });

        this.colProduto.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        this.colProduto.SortMode = DataGridViewColumnSortMode.NotSortable;

        this.colQuantidade.Width = 140;
        this.colQuantidade.SortMode = DataGridViewColumnSortMode.NotSortable;
        this.colQuantidade.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        this.colQuantidade.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

        this.btnRemoverItem.Location = new System.Drawing.Point(18, 314);
        this.btnRemoverItem.Size = new System.Drawing.Size(176, 36);
        Theme.SecondaryButton(this.btnRemoverItem);
        this.btnRemoverItem.Click += new EventHandler(this.btnRemoverItem_Click);

        this.btnExecutar.Location = new System.Drawing.Point(530, 312);
        this.btnExecutar.Size = new System.Drawing.Size(172, 40);
        this.btnExecutar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        Theme.PrimaryButton(this.btnExecutar);
        this.btnExecutar.Click += new EventHandler(this.btnExecutar_Click);

        // ---------- Cartão do registro ----------

        this.pnlLog.Location = new System.Drawing.Point(20, 480);
        this.pnlLog.Size = new System.Drawing.Size(720, 200);
        this.pnlLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.pnlLog.Controls.Add(this.lblLogTitle);
        this.pnlLog.Controls.Add(this.pnlLogBox);

        this.lblLogTitle.AutoSize = true;
        this.lblLogTitle.Font = Theme.CardTitle;
        this.lblLogTitle.ForeColor = Theme.Text;
        this.lblLogTitle.Location = new System.Drawing.Point(18, 14);

        this.pnlLogBox.BackColor = Theme.LogBackground;
        this.pnlLogBox.Padding = new Padding(12, 10, 12, 10);
        this.pnlLogBox.Location = new System.Drawing.Point(18, 46);
        this.pnlLogBox.Size = new System.Drawing.Size(684, 138);
        this.pnlLogBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.pnlLogBox.Controls.Add(this.txtLog);

        this.txtLog.Dock = DockStyle.Fill;
        this.txtLog.BorderStyle = BorderStyle.None;
        this.txtLog.BackColor = Theme.LogBackground;
        this.txtLog.ForeColor = ColorTranslator.FromHtml("#1F2937");
        this.txtLog.Multiline = true;
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = ScrollBars.Vertical;
        this.txtLog.Font = Theme.Log;

        // ---------- Janela ----------

        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        this.BackColor = Theme.Background;
        this.Font = Theme.Base;
        this.ClientSize = new System.Drawing.Size(760, 740);
        this.MinimumSize = new System.Drawing.Size(700, 660);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.lblSubtitle);
        this.Controls.Add(this.pillStatus);
        this.Controls.Add(this.btnTestConnection);
        this.Controls.Add(this.btnSignOut);
        this.Controls.Add(this.pnlSetup);
        this.Controls.Add(this.pnlLote);
        this.Controls.Add(this.pnlLog);

        ((System.ComponentModel.ISupportInitialize)(this.numQuantidade)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
        this.pnlSetup.ResumeLayout(false);
        this.pnlSetup.PerformLayout();
        this.pnlLote.ResumeLayout(false);
        this.pnlLote.PerformLayout();
        this.pnlLogBox.ResumeLayout(false);
        this.pnlLogBox.PerformLayout();
        this.pnlLog.ResumeLayout(false);
        this.pnlLog.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
