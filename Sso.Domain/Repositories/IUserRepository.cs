using Sso.Domain.Entities;
using Sso.Domain.ValueObjects;

namespace Sso.Domain.Repositories;

public interface IUserRepository 
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = dafault);

    Task<User?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = dafault);

    Task AddAsyc(User user, CancellationToken cancellationToken = dafault)
}