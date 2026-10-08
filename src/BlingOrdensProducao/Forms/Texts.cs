namespace BlingOrdensProducao.Forms;

/// <summary>
/// Textos da interface. O padrão é português; com APP_LANG=en (ou o argumento --lang en)
/// a tela fica em inglês, usado para prints de portfólio.
/// </summary>
internal static class Texts
{
    public static bool English { get; set; } =
        string.Equals(Environment.GetEnvironmentVariable("APP_LANG"), "en", StringComparison.OrdinalIgnoreCase);

    private static string T(string pt, string en) => English ? en : pt;

    public static string WindowTitle => T("Bling - Ordens de Produção", "Bling - Production Orders");
    public static string Title => T("Ordens de Produção", "Production Orders");
    public static string Subtitle => T("Crie e finalize ordens de produção no Bling em lote", "Create and close Bling production orders in batch");
    public static string Connected => T("Conectado ao Bling", "Connected to Bling");
    public static string TestConnection => T("Testar conexão", "Test connection");
    public static string SignOut => T("Desconectar", "Sign out");

    public static string SetupTitle => T("Conecte sua conta do Bling", "Connect your Bling account");
    public static string SetupHint => T("Informe os dados do aplicativo cadastrado no painel do Bling.", "Enter the app credentials registered in the Bling dashboard.");
    public static string ClientId => "Client ID";
    public static string ClientSecret => "Client Secret";
    public static string Authorize => T("Autorizar acesso ao Bling", "Authorize Bling access");

    public static string BatchTitle => T("Lote de produção", "Production batch");
    public static string ProductLabel => T("Produto (digite para buscar)", "Product (type to search)");
    public static string LoadingCatalog => T("Carregando catálogo de produtos...", "Loading product catalog...");
    public static string CatalogFailed => T("Produto (o catálogo não carregou)", "Product (catalog did not load)");
    public static string RefreshCatalog => T("Atualizar catálogo", "Refresh catalog");
    public static string Quantity => T("Quantidade", "Quantity");
    public static string AddItem => T("Adicionar à lista", "Add to batch");
    public static string ColumnProduct => T("Produto", "Product");
    public static string ColumnQuantity => T("Quantidade", "Quantity");
    public static string RemoveItem => T("Remover selecionado", "Remove selected");
    public static string Execute => T("Executar lote", "Run batch");
    public static string LogTitle => T("Registro", "Activity log");
    public static string Stock => T("estoque", "stock");

    public static string BatchSummary(int items, int units) => English
        ? $"{items} {(items == 1 ? "item" : "items")} · {units} units"
        : $"{items} {(items == 1 ? "item" : "itens")} · {units} unidades";

    // Registro
    public static string LogLoadingCatalog => T("Carregando catálogo de produtos do Bling...", "Loading the Bling product catalog...");
    public static string LogCatalogLoaded(int total) => T($"Catálogo carregado: {total} produtos.", $"Catalog loaded: {total} products.");
    public static string LogCatalogError(string message) => T($"Erro ao carregar catálogo de produtos: {message}", $"Error loading the product catalog: {message}");
    public static string LogOpeningBrowser => T("Abrindo o navegador para autorização no Bling...", "Opening the browser for Bling authorization...");
    public static string LogAuthorized => T("Autorização concluída com sucesso. Tokens salvos localmente.", "Authorization completed. Tokens saved locally.");
    public static string LogAuthFailed(string message) => T($"Falha na autorização: {message}", $"Authorization failed: {message}");
    public static string LogTesting => T("Testando conexão com a API do Bling...", "Testing the connection to the Bling API...");
    public static string LogConnectionOk => T("Conexão OK: a API do Bling respondeu normalmente.", "Connection OK: the Bling API responded normally.");
    public static string LogConnectionFailed(string message) => T($"Falha ao testar conexão: {message}", $"Connection test failed: {message}");
    public static string LogSignedOut => T("Desconectado. Token local removido.", "Signed out. Local token removed.");
    public static string LogStarting(int count) => T($"Iniciando execução de {count} item(ns)...", $"Running {count} item(s)...");
    public static string LogItemOk(string name, int quantity, long orderId) => T(
        $"OK: {name} x{quantity}, ordem de produção #{orderId} criada e finalizada.",
        $"OK: {name} x{quantity}, production order #{orderId} created and closed.");
    public static string LogItemError(string name, int quantity, string message) => T(
        $"ERRO ({name} x{quantity}): {message}", $"ERROR ({name} x{quantity}): {message}");
    public static string LogBatchDone => T("Lote concluído: todos os itens foram processados com sucesso.", "Batch finished: every item was processed successfully.");
    public static string LogBatchPending(int count) => T(
        $"Lote processado com {count} item(ns) pendente(s) (falharam). Corrija e tente executar de novo.",
        $"Batch processed with {count} pending item(s) (failed). Fix them and run again.");

    // Diálogos
    public static string IncompleteTitle => T("Dados incompletos", "Missing data");
    public static string IncompleteMessage => T("Informe o Client ID e o Client Secret do aplicativo cadastrado no Bling.", "Enter the Client ID and Client Secret of the app registered in Bling.");
    public static string AuthFailedTitle => T("Falha na autorização", "Authorization failed");
    public static string SignOutConfirm => T("Isso vai apagar o token salvo localmente. Será necessário autorizar novamente. Continuar?", "This deletes the locally saved token. You will need to authorize again. Continue?");
    public static string NoProductTitle => T("Nenhum produto selecionado", "No product selected");
    public static string NoProductMessage => T("Busque e selecione um produto da lista de sugestões antes de adicionar.", "Search and pick a product from the suggestions before adding it.");
    public static string EmptyTitle => T("Lista vazia", "Empty batch");
    public static string EmptyMessage => T("Adicione ao menos um item à lista antes de executar.", "Add at least one item to the batch before running it.");
    public static string ConfirmTitle => T("Confirmar execução", "Confirm run");
    public static string ConfirmMessage(int count) => T(
        $"Isso vai criar e finalizar {count} ordem(ns) de produção no Bling agora, creditando o estoque de cada produto. Continuar?",
        $"This will create and close {count} production order(s) in Bling now, adding stock for each product. Continue?");
}
