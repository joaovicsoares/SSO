using Sso.Application.DTOs;

namespace Sso.Application.Authentication;

public interface IAuthenticationService
{
    Task<LoginResponse?> ValidateCredentialsAsync(
        LoginRequest loginRequest,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default
    );

    Task<LoginResponse?> RegisterUserAsync(
        RegisterRequest request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task<LoginResponse?> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);
}