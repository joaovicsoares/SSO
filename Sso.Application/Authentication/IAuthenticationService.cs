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
}