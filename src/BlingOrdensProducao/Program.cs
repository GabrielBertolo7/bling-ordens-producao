using System.Text.Json;
using BlingOrdensProducao.Forms;
using BlingOrdensProducao.Services;

namespace BlingOrdensProducao;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--diag")
        {
            RunDiagnostic(args).GetAwaiter().GetResult();
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }

    // Modo de linha de comando pra consultar endpoints da API reaproveitando o token já salvo,
    // sem precisar da UI. Não é usado pelo usuário final.
    // Uso: dotnet run --project src/BlingOrdensProducao -- --diag modulos|situacoes {idModulo}|depositos|ultima-ordem
    // Resultado é gravado em diag-output.txt no diretório atual.
    private static async Task RunDiagnostic(string[] args)
    {
        const string outputFile = "diag-output.txt";

        var oAuthService = new BlingOAuthService(new SecureTokenStorage());
        if (!oAuthService.IsAuthorized)
        {
            await File.WriteAllTextAsync(outputFile, "ERRO: app ainda não autorizado. Abra a tela principal e autorize primeiro.");
            return;
        }

        var apiClient = new BlingApiClient(oAuthService);
        var subcommand = args.Length > 1 ? args[1] : "modulos";

        string path;
        if (subcommand == "modulos")
        {
            path = "/situacoes/modulos";
        }
        else if (subcommand == "situacoes" && args.Length > 2)
        {
            path = $"/situacoes/modulos/{args[2]}";
        }
        else if (subcommand == "depositos")
        {
            path = "/depositos";
        }
        else if (subcommand == "ultima-ordem")
        {
            path = "/ordens-producao?limite=100&pagina=1";
        }
        else if (subcommand == "produtos" && args.Length > 2)
        {
            path = $"/produtos?criterio={Uri.EscapeDataString(args[2])}&limite=5";
        }
        else if (subcommand == "raw" && args.Length > 2)
        {
            path = args[2];
        }
        else if ((subcommand == "patch" || subcommand == "put" || subcommand == "post") && args.Length > 3)
        {
            var writePath = args[2];
            var bodyJson = args[3];

            try
            {
                var payload = JsonDocument.Parse(bodyJson).RootElement;
                var raw = subcommand switch
                {
                    "patch" => await apiClient.PatchRawAsync(writePath, payload),
                    "put" => await apiClient.PutRawAsync(writePath, payload),
                    _ => await apiClient.PostRawAsync(writePath, payload),
                };
                var pretty = JsonSerializer.Serialize(JsonDocument.Parse(raw).RootElement, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(outputFile, pretty);
            }
            catch (Exception ex)
            {
                await File.WriteAllTextAsync(outputFile, $"ERRO: {ex.Message}");
            }

            return;
        }
        else
        {
            await File.WriteAllTextAsync(outputFile, $"ERRO: subcomando inválido '{subcommand}'.");
            return;
        }

        try
        {
            var raw = await apiClient.GetRawAsync(path);
            var pretty = JsonSerializer.Serialize(JsonDocument.Parse(raw).RootElement, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(outputFile, pretty);
        }
        catch (Exception ex)
        {
            await File.WriteAllTextAsync(outputFile, $"ERRO: {ex.Message}");
        }
    }
}