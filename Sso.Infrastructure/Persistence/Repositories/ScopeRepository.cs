using Microsoft.EntityFrameworkCore;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;

namespace Sso.Infrastructure.Persistence.Repositories
{
    public class ScopeRepository(SsoDbContext context) : IScopeRepository
    {
        public void Add(Scope scope)
        {
            context.Scopes.Add(scope);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            return await context.Scopes.AnyAsync(cancellationToken: cancellationToken);
        }
    }
}
