using System.Drawing.Drawing2D;

namespace BlingOrdensProducao.Forms;

/// <summary>Cores, fontes e estilos de controle da interface (claro, sóbrio, um tom de destaque).</summary>
internal static class Theme
{
    public static readonly Color Background = ColorTranslator.FromHtml("#F4F6F8");
    public static readonly Color Surface = Color.White;
    public static readonly Color Border = ColorTranslator.FromHtml("#E5E7EB");
    public static readonly Color FieldBorder = ColorTranslator.FromHtml("#D1D5DB");
    public static readonly Color Text = ColorTranslator.FromHtml("#111827");
    public static readonly Color Muted = ColorTranslator.FromHtml("#6B7280");
    public static readonly Color Primary = ColorTranslator.FromHtml("#1D4ED8");
    public static readonly Color PrimaryDark = ColorTranslator.FromHtml("#1E40AF");
    public static readonly Color PrimarySoft = ColorTranslator.FromHtml("#DBEAFE");
    public static readonly Color SuccessSoft = ColorTranslator.FromHtml("#DCFCE7");
    public static readonly Color Success = ColorTranslator.FromHtml("#166534");
    public static readonly Color LogBackground = ColorTranslator.FromHtml("#F9FAFB");

    public static readonly Font Base = new("Segoe UI", 9.75F);
    public static readonly Font Title = new("Segoe UI", 16F, FontStyle.Bold);
    public static readonly Font Subtitle = new("Segoe UI", 9.75F);
    public static readonly Font CardTitle = new("Segoe UI Semibold", 11F);
    public static readonly Font Label = new("Segoe UI Semibold", 9F);
    public static readonly Font Button = new("Segoe UI Semibold", 9.75F);
    public static readonly Font Input = new("Segoe UI", 10.5F);
    public static readonly Font Log = new("Cascadia Mono", 9F);

    public static void PrimaryButton(Button button) => Style(button, Primary, Color.White, Primary, PrimaryDark);

    public static void SecondaryButton(Button button) => Style(button, Surface, Text, FieldBorder, ColorTranslator.FromHtml("#F3F4F6"));

    private static void Style(Button button, Color back, Color fore, Color border, Color hover)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.BackColor = back;
        button.ForeColor = fore;
        button.Font = Button;
        button.Cursor = Cursors.Hand;
        button.FlatAppearance.BorderColor = border;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.MouseOverBackColor = hover;
        button.FlatAppearance.MouseDownBackColor = hover;
        button.UseVisualStyleBackColor = false;
    }

    public static void Grid(DataGridView grid)
    {
        grid.BackgroundColor = Surface;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.GridColor = Border;
        grid.RowHeadersVisible = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = 36;
        grid.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FAFAFA");
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Muted;
        grid.ColumnHeadersDefaultCellStyle.Font = Label;
        grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#FAFAFA");
        grid.DefaultCellStyle.Font = Base;
        grid.DefaultCellStyle.ForeColor = Text;
        grid.DefaultCellStyle.BackColor = Surface;
        grid.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
        grid.DefaultCellStyle.SelectionBackColor = PrimarySoft;
        grid.DefaultCellStyle.SelectionForeColor = Text;
        grid.RowTemplate.Height = 36;
    }
}

/// <summary>Painel branco com borda fina e cantos arredondados (os "cartões" da tela).</summary>
internal sealed class CardPanel : Panel
{
    public CardPanel()
    {
        DoubleBuffered = true;
        BackColor = Theme.Surface;
        Padding = new Padding(18, 14, 18, 16);
        SetStyle(ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? Theme.Background);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = RoundedRect(rect, 10);
        using var fill = new SolidBrush(BackColor);
        using var pen = new Pen(Theme.Border);
        e.Graphics.FillPath(fill, path);
        e.Graphics.DrawPath(pen, path);
    }

    private static GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        var d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

/// <summary>Selo arredondado com bolinha colorida (ex.: "Conectado ao Bling").</summary>
internal sealed class StatusPill : Control
{
    public StatusPill()
    {
        DoubleBuffered = true;
        Font = Theme.Label;
        BackColor = Theme.SuccessSoft;
        ForeColor = Theme.Success;
        SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        var size = TextRenderer.MeasureText(Text, Font);
        Size = new Size(size.Width + 34, 26);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? Theme.Background);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var fill = new SolidBrush(BackColor);
        using var path = new GraphicsPath();
        var h = Height - 1;
        path.AddArc(0, 0, h, h, 90, 180);
        path.AddArc(Width - 1 - h, 0, h, h, 270, 180);
        path.CloseFigure();
        e.Graphics.FillPath(fill, path);
        using var dot = new SolidBrush(ColorTranslator.FromHtml("#22C55E"));
        e.Graphics.FillEllipse(dot, 11, (Height - 8) / 2, 8, 8);
        TextRenderer.DrawText(e.Graphics, Text, Font, new Rectangle(24, 0, Width - 28, Height), ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
    }
}
