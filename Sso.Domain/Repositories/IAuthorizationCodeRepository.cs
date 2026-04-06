using Sso.Domain.Entities;

namespace Sso.Domain.Repositories;

public interface IAuthorizationCodeRepository
{
    void Add(AuthorizationCode code);
    Task<AuthorizationCode?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
