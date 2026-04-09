using System.Security.Cryptography;
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
        // Buscar usuário
        var user = await _userRepository.GetByGuidAsync(userId, cancellationToken);
        if (user == null || !user.IsActive)
            throw new InvalidOperationException("User not found or inactive");

        // TODO: Buscar client real - por enquanto, criar um client temporário
        // Isso precisa ser implementado quando houver um ClientRepository
        var client = new Client
        {
            Name = "Default Client",
            ClientId = "default-client",
            ClientSecret = "secret",
            RedirectUris = "http://localhost:3000/callback"
        };

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
            User = user,
            Client = client,
            RedirectUri = client.RedirectUris, // TODO: validar redirect_uri do request
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

        // 3. Marcar código como usado (previne replay attacks)
        authCode.IsUsed = true;

        // 4. Buscar usuário
        var user = authCode.User;
        if (user == null || !user.IsActive)
            return null;

        // 5. Gerar os três tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var idToken = _jwtService.GenerateIdToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // 6. Armazenar refresh token no banco (para controle e revogação futura)
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            User = user,
            Client = authCode.Client,
            ExpiresAt = DateTime.UtcNow.AddDays(30), // Refresh token dura 30 dias
            IsRevoked = false
        };

        _refreshTokenRepository.Add(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Retornar todos os tokens
        return new TokenResponse(
            accessToken, 
            idToken, 
            refreshToken, 
            900 // expires_in: 900 segundos = 15 minutos
        );
    }
}
