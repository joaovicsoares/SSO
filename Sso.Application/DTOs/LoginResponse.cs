namespace Sso.Application.DTOs;

public record LoginResponse(
    string Guid, 
    string Email, 
    string? Name,
    string AccessToken,
    string IdToken,
    string RefreshToken,
    int ExpiresIn);