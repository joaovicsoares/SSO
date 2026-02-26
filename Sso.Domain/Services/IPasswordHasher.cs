namespace Sso.Domain.Services;

/// <summary>
/// Interface for password hashing operations.
/// Provides methods to hash passwords and verify them against stored hashes.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a password using a secure algorithm with automatic salt generation.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>The hashed password with embedded salt.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hash">The stored hash to verify against.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    bool VerifyPassword(string password, string hash);
}
