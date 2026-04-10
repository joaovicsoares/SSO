using Sso.Domain.Repositories;
using Sso.Domain.Services;
using Sso.Application.Persistence;
using Sso.Application.DTOs;
using Sso.Domain.Entities;
using Sso.Domain.Enums;
using Sso.Domain.ValueObjects;

namespace Sso.Application.Authentication;

public class AuthenticationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IAuditLogRepository auditLogRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtService jwtService,
    IUnitOfWork unitOfWork,
    IClientRepository clientRepository
    ) : IAuthenticationService
{
    // Hash pré-computado usado para manter tempo de resposta constante
    // quando o email não existe (prevenção de timing attack)
    private const string DummyHash =
        "$argon2id$v=19$m=65536,t=4,p=1$AAAAAAAAAAAAAAAAAAAAAA==$AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

    public async Task<LoginResponse?> ValidateCredentialsAsync(
        LoginRequest loginRequest,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        if (!Email.IsValid(loginRequest.Email))
        {
            passwordHasher.VerifyPassword(loginRequest.Password, DummyHash);
            await LogAsync(EventType.LoginFailed, ipAddress, userAgent);
            return null;
        }
        var email = new Email(loginRequest.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            passwordHasher.VerifyPassword(loginRequest.Password, DummyHash);
            await LogAsync(EventType.LoginFailed, ipAddress, userAgent);
            return null;
        }

        if (!user.IsActive)
        {
            await LogAsync(EventType.LoginFailed, ipAddress, userAgent, user);
            return null;
        }

        if (!passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash))
        {
            await LogAsync(EventType.LoginFailed, ipAddress, userAgent, user);
            return null;
        }

        // Generate JWT tokens
        var accessToken = jwtService.GenerateAccessToken(user);
        var idToken = jwtService.GenerateIdToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        // Store refresh token in database
        var client = await GetOrCreateDefaultClientAsync(cancellationToken);
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            User = user,
            Client = client,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false
        };
        refreshTokenRepository.Add(refreshTokenEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await LogAsync(EventType.LoginSuccess, ipAddress, userAgent, user);
        return new LoginResponse(
            user.Guid.ToString(), 
            user.Email.Value, 
            user.Name,
            accessToken,
            idToken,
            refreshToken,
            900); // 15 minutes in seconds
    }

    public async Task<LoginResponse?> RegisterUserAsync(
        RegisterRequest request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        // Validate email format
        if (!Email.IsValid(request.Email))
        {
            await LogAsync(EventType.UserCreationFailed, ipAddress, userAgent);
            return null;
        }

        var email = new Email(request.Email);

        // Check if user already exists
        var existingUser = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser != null)
        {
            await LogAsync(EventType.UserCreationFailed, ipAddress, userAgent);
            return null;
        }

        // Hash password
        var passwordHash = passwordHasher.HashPassword(request.Password);

        // Create user with required properties
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Name = request.Name,
            IsActive = true
        };

        await userRepository.AddAsyc(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Log registration
        await LogAsync(EventType.UserCreated, ipAddress, userAgent, user);

        // Generate JWT tokens for the new user
        var accessToken = jwtService.GenerateAccessToken(user);
        var idToken = jwtService.GenerateIdToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();

        // Store refresh token in database
        var client = await GetOrCreateDefaultClientAsync(cancellationToken);
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            User = user,
            Client = client,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false
        };
        refreshTokenRepository.Add(refreshTokenEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            user.Guid.ToString(), 
            user.Email.Value, 
            user.Name,
            accessToken,
            idToken,
            refreshToken,
            900); // 15 minutes in seconds
    }

    public async Task<LoginResponse?> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        // Find the refresh token in the database
        var tokenEntity = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

        // Validate refresh token
        if (tokenEntity == null || 
            tokenEntity.IsRevoked || 
            tokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            await LogAsync(EventType.LoginFailed, ipAddress, userAgent);
            return null;
        }

        // Get the user
        var user = tokenEntity.User;
        if (user == null || !user.IsActive)
        {
            await LogAsync(EventType.LoginFailed, ipAddress, userAgent);
            return null;
        }

        // Generate new tokens
        var accessToken = jwtService.GenerateAccessToken(user);
        var idToken = jwtService.GenerateIdToken(user);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        // Revoke old refresh token
        tokenEntity.IsRevoked = true;

        // Store new refresh token
        var newRefreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            User = user,
            Client = tokenEntity.Client,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            IsRevoked = false
        };

        refreshTokenRepository.Add(newRefreshTokenEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await LogAsync(EventType.LoginSuccess, ipAddress, userAgent, user);

        return new LoginResponse(
            user.Guid.ToString(),
            user.Email.Value,
            user.Name,
            accessToken,
            idToken,
            newRefreshToken,
            900);
    }

    private async Task<Client> GetOrCreateDefaultClientAsync(CancellationToken cancellationToken = default)
    {
        const string defaultClientId = "default-client";
        
        var existingClient = await clientRepository.GetByClientIdAsync(defaultClientId, cancellationToken);

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

        await clientRepository.AddAsync(newClient, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return newClient;
    }

    private async Task LogAsync(
        EventType eventType,
        string? ipAddress,
        string? userAgent,
        User? user = null)
    {
        var log = new AuditLog
        {
            EventType = eventType,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            User = user
        };

        auditLogRepository.Add(log);
        await unitOfWork.SaveChangesAsync();
    }
}