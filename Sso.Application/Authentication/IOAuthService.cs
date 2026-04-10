namespace Sso.Application.Authentication;

public interface IOAuthService
{
    Task<string> GenerateAuthorizationCodeAsync(
        Guid userId,
        string? codeChallenge,
        string? codeChallengeMethod,
        CancellationToken cancellationToken = default);

    Task<TokenResponse?> ExchangeCodeForTokensAsync(
        string code,
        string? codeVerifier,
        CancellationToken cancellationToken = default);
}

public record TokenResponse(
    string AccessToken,
    string IdToken,
    string RefreshToken,
    int ExpiresIn,
    string TokenType = "Bearer");
