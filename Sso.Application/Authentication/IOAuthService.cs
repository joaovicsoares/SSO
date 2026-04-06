namespace Sso.Application.Authentication;

public interface IOAuthService
{
    /// <summary>
    /// Gera um authorization code vinculado ao usuário com suporte a PKCE
    /// </summary>
    Task<string> GenerateAuthorizationCodeAsync(
        Guid userId,
        string? codeChallenge,
        string? codeChallengeMethod,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Troca um authorization code por tokens JWT (Access, ID, Refresh)
    /// </summary>
    Task<TokenResponse?> ExchangeCodeForTokensAsync(
        string code,
        string? codeVerifier,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Resposta contendo todos os tokens OAuth 2.0
/// </summary>
public record TokenResponse(
    string AccessToken,
    string IdToken,
    string RefreshToken,
    int ExpiresIn,
    string TokenType = "Bearer");
