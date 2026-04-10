using Sso.Domain.Entities;

namespace Sso.Application.Authentication;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateIdToken(User user);
    string GenerateRefreshToken();
}
