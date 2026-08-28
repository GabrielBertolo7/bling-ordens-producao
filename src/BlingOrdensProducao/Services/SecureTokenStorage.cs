using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BlingOrdensProducao.Models;

namespace BlingOrdensProducao.Services;

// Persiste client_id/client_secret/tokens em disco, criptografados via DPAPI (Windows Data
// Protection API) e atrelados ao usuário do Windows que executa o app. Fica em %AppData%, fora
// da pasta do projeto, nunca em texto puro e nunca versionado no git.
public sealed class SecureTokenStorage
{
    private static readonly string StorageDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "BlingOrdensProducao");

    private static readonly string StorageFilePath = Path.Combine(StorageDirectory, "auth.dat");

    public bool Exists() => File.Exists(StorageFilePath);

    public StoredAuthData? Load()
    {
        if (!File.Exists(StorageFilePath))
        {
            return null;
        }

        var encryptedBytes = File.ReadAllBytes(StorageFilePath);
        var jsonBytes = ProtectedData.Unprotect(encryptedBytes, optionalEntropy: null, DataProtectionScope.CurrentUser);
        var json = Encoding.UTF8.GetString(jsonBytes);
        return JsonSerializer.Deserialize<StoredAuthData>(json);
    }

    public void Save(StoredAuthData data)
    {
        Directory.CreateDirectory(StorageDirectory);

        var json = JsonSerializer.Serialize(data);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var encryptedBytes = ProtectedData.Protect(jsonBytes, optionalEntropy: null, DataProtectionScope.CurrentUser);

        File.WriteAllBytes(StorageFilePath, encryptedBytes);
    }

    public void Clear()
    {
        if (File.Exists(StorageFilePath))
        {
            File.Delete(StorageFilePath);
        }
    }
}
