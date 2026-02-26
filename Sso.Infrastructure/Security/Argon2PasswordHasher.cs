using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Sso.Domain.Services;

namespace Sso.Infrastructure.Security;

/// <summary>
/// Implementation of password hashing using Argon2id algorithm.
/// Argon2id is recommended by OWASP as the best password hashing algorithm (2024).
/// It provides resistance against GPU attacks and side-channel attacks.
/// </summary>
public class Argon2PasswordHasher : IPasswordHasher
{
    // Secure parameters recommended by OWASP
    private const int MemoryCost = 65536; // 64 MB - Makes memory attacks difficult
    private const int Iterations = 4; // Balances security and performance
    private const int Parallelism = 1; // Suitable for server environments
    private const int HashLength = 32; // 256 bits
    private const int SaltLength = 16; // 128 bits

    /// <summary>
    /// Hashes a password using Argon2id with automatic salt generation.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password in format: $argon2id$v=19$m=65536,t=4,p=1$&lt;salt&gt;$&lt;hash&gt;</returns>
    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        // Generate unique salt
        byte[] salt = GenerateSalt();
        
        // Hash the password
        byte[] hash = HashPasswordInternal(password, salt);
        
        // Return in standard format
        return FormatHash(salt, hash);
    }

    /// <summary>
    /// Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hash">The stored hash to verify against.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        if (string.IsNullOrEmpty(hash))
        {
            return false;
        }

        try
        {
            // Parse the hash to extract salt and hash bytes
            var (salt, storedHash) = ParseHash(hash);
            
            // Hash the provided password with the extracted salt
            byte[] computedHash = HashPasswordInternal(password, salt);
            
            // Compare hashes in constant time to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
        catch
        {
            // If parsing fails or any error occurs, return false
            return false;
        }
    }

    /// <summary>
    /// Generates a cryptographically secure random salt.
    /// </summary>
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltLength];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }

    /// <summary>
    /// Hashes a password with the given salt using Argon2id.
    /// </summary>
    private static byte[] HashPasswordInternal(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = Parallelism,
            MemorySize = MemoryCost,
            Iterations = Iterations
        };

        return argon2.GetBytes(HashLength);
    }

    /// <summary>
    /// Formats the hash in the standard Argon2 format.
    /// Format: $argon2id$v=19$m=65536,t=4,p=1$&lt;base64_salt&gt;$&lt;base64_hash&gt;
    /// </summary>
    private static string FormatHash(byte[] salt, byte[] hash)
    {
        string saltBase64 = Convert.ToBase64String(salt);
        string hashBase64 = Convert.ToBase64String(hash);
        
        return $"$argon2id$v=19$m={MemoryCost},t={Iterations},p={Parallelism}${saltBase64}${hashBase64}";
    }

    /// <summary>
    /// Parses a hash string to extract salt and hash bytes.
    /// </summary>
    private static (byte[] salt, byte[] hash) ParseHash(string hashString)
    {
        string[] parts = hashString.Split('$');
        
        if (parts.Length != 6)
        {
            throw new FormatException("Invalid hash format.");
        }

        if (parts[1] != "argon2id")
        {
            throw new FormatException("Hash is not Argon2id.");
        }

        byte[] salt = Convert.FromBase64String(parts[4]);
        byte[] hash = Convert.FromBase64String(parts[5]);

        return (salt, hash);
    }
}
