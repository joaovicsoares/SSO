using Sso.Domain.Entities;

namespace Sso.Domain.Repositories
{
    public interface IScopeRepository
    {
        void Add(Scope scope);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
