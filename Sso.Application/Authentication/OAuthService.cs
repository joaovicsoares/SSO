using System.Security.Cryptography;
using System.Text;
using Sso.Application.Persistence;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;

namespace Sso.Application.Authentication;

public class OAuthService : IOAuthService
{
    private readonly IAuthorizationCodeRepository _authCodeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public OAuthService(
        IAuthorizationCodeRepository authCodeRepository,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork)
    {
        _authCodeRepository = authCodeRepository;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateAuthorizationCodeAsync(
        Guid userId,
        string? codeChallenge,
        string? codeChallengeMethod,
        CancellationToken cancellationToken = default)
    {
        // Gerar código aleatório criptograficamente seguro (32 bytes = 256 bits)
        var codeBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(codeBytes);
        
        // Converter para Base64Url (sem caracteres +, /, = que podem causar problemas em URLs)
        var code = Convert.ToBase64String(codeBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        // Criar entidade AuthorizationCode
        var authCode = new AuthorizationCode
        {
            Code = code,
            UserId = userId,
            CodeChallenge = codeChallenge,
            CodeChallengeMethod = codeChallengeMethod,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10), // Código expira em 10 minutos (padrão OAuth)
            IsUsed = false
        };

        _authCodeRepository.Add(authCode);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return code;
    }

    public async Task<TokenResponse?> ExchangeCodeForTokensAsync(
        string code,
        string? codeVerifier,
        CancellationToken cancellationToken = default)
    {
        // 1. Buscar authorization code no banco
        var authCode = await _authCodeRepository.GetByCodeAsync(code, cancellationToken);

        // 2. Validar authorization code
        // - Código deve existir
        // - Não pode ter sido usado antes (uso único!)
        // - Não pode estar expirado
        if (authCode == null || authCode.IsUsed || authCode.ExpiresAt < DateTime.UtcNow)
            return null;

        // 3. Validar PKCE se code_challenge foi fornecido no /authorize
        if (authCode.CodeChallenge != null)
        {
            // Se tinha code_challenge, DEVE ter code_verifier agora
            if (codeVerifier == null)
                return null;

            // Validar usando o método correto (S256 ou plain)
            var isValid = authCode.CodeChallengeMethod?.ToLower() == "s256"
                ? ValidateS256(codeVerifier, authCode.CodeChallenge)
                : ValidatePlain(codeVerifier, authCode.CodeChallenge);

            if (!isValid)
                return null;
        }

        // 4. Marcar código como usado (previne replay attacks)
        authCode.IsUsed = true;

        // 5. Buscar usuário
        var user = authCode.User ?? await _userRepository.GetByGuidAsync(authCode.UserId, cancellationToken);
        if (user == null || !user.IsActive)
            return null;

        // 6. Gerar os três tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var idToken = _jwtService.GenerateIdToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // 7. Armazenar refresh token no banco (para controle e revogação futura)
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30), // Refresh token dura 30 dias
            IsRevoked = false
        };

        _refreshTokenRepository.Add(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 8. Retornar todos os tokens
        return new TokenResponse(
            accessToken, 
            idToken, 
            refreshToken, 
            900 // expires_in: 900 segundos = 15 minutos
        );
    }

    /// <summary>
    /// Valida PKCE usando SHA256 (método S256)
    /// </summary>
    private static bool ValidateS256(string verifier, string challenge)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(verifier));
        var computedChallenge = Convert.ToBase64String(hash)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
        return computedChallenge == challenge;
    }

    /// <summary>
    /// Valida PKCE usando método plain (sem hash)
    /// </summary>
    private static bool ValidatePlain(string verifier, string challenge)
    {
        return verifier == challenge;
    }
}
