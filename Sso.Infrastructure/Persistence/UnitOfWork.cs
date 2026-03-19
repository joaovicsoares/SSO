using Sso.Application.Persistence;

namespace Sso.Infrastructure.Persistence
{
    public class UnitOfWork(SsoDbContext context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);
    }
}
