using Sso.Domain.Entities;

namespace Sso.Domain.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
}
