using Microsoft.EntityFrameworkCore;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;
using Sso.Domain.ValueObjects;

namespace Sso.Infrastructure.Persistence.Repositories;

public class UserRepository(SsoDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(
            x => x.Email == email, cancellationToken
        );
    }

    public async Task<User?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(
            x => x.Guid == guid, cancellationToken
        );
    }

    public async Task AddAsyc(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }
}