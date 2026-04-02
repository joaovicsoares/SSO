using System.Security.Cryptography;

var rsa = RSA.Create(2048);
var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());

Console.WriteLine("=== RSA Keys Generated ===");
Console.WriteLine();
Console.WriteLine("Private Key:");
Console.WriteLine(privateKey);
Console.WriteLine();
Console.WriteLine("Public Key:");
Console.WriteLine(publicKey);
Console.WriteLine();
Console.WriteLine("Add these to your appsettings.json under 'Jwt' section");
