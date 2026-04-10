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
    private readonly IClientRepository _clientRepository;

    public OAuthService(
        IAuthorizationCodeRepository authCodeRepository,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        IClientRepository clientRepository)
    {
        _authCodeRepository = authCodeRepository;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
    }

    public async Task<string> GenerateAuthorizationCodeAsync(
        Guid userId,
        string? codeChallenge,
        string? codeChallengeMethod,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByGuidAsync(userId, cancellationToken);
        if (user == null || !user.IsActive)
            throw new InvalidOperationException("User not found or inactive");

        var client = await GetOrCreateDefaultClientAsync(cancellationToken);

        var codeBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(codeBytes);
        
        var code = Convert.ToBase64String(codeBytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        var authCode = new AuthorizationCode
        {
            Code = code,
            User = user,
            Client = client,
            RedirectUri = client.RedirectUris,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
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
        var authCode = await _authCodeRepository.GetByCodeAsync(code, cancellationToken);

        if (authCode == null || authCode.IsUsed || authCode.ExpiresAt < DateTime.UtcNow)
            return null;

        authCode.IsUsed = true;

        var user = authCode.User;
        if (user == null || !user.IsActive)
            return null;

        var accessToken = _jwtService.GenerateAccessToken(user);
        var idToken = _jwtService.GenerateIdToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            User = user,
            Client = authCode.Client,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false
        };

        _refreshTokenRepository.Add(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenResponse(
            accessToken, 
            idToken, 
            refreshToken, 
            900
        );
    }

    private async Task<Client> GetOrCreateDefaultClientAsync(CancellationToken cancellationToken = default)
    {
        const string defaultClientId = "default-client";
        
        var existingClient = await _clientRepository.GetByClientIdAsync(defaultClientId, cancellationToken);

        if (existingClient != null)
        {
            return existingClient;
        }

        var newClient = new Client
        {
            Name = "Default Client",
            ClientId = defaultClientId,
            ClientSecret = "secret",
            RedirectUris = "http://localhost:3000/callback"
        };

        await _clientRepository.AddAsync(newClient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newClient;
    }
}
