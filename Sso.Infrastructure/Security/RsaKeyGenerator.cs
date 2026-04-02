using System.Security.Cryptography;

namespace Sso.Infrastructure.Security;

public static class RsaKeyGenerator
{
    public static (string privateKey, string publicKey) GenerateKeys()
    {
        using var rsa = RSA.Create(2048);
        
        var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
        var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        
        return (privateKey, publicKey);
    }
}
