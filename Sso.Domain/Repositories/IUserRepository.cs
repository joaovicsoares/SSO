using Sso.Domain.Entities;
using Sso.Domain.ValueObjects;

namespace Sso.Domain.Repositories;

public interface IUserRepository 
{
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<User?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task AddAsyc(User user, CancellationToken cancellationToken = default);
}