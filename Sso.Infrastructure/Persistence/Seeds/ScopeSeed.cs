using Sso.Application.Persistence;
using Sso.Domain.Repositories;
using Sso.Domain.Services;

namespace Sso.Infrastructure.Persistence.Seeds
{
    public class ScopeSeed(ScopeService scopeService, IScopeRepository scopeRepository, IUnitOfWork unitOfWork)
    {
        public async Task SeedAsync()
        {
            if (await scopeRepository.AnyAsync())
                return;

            scopeService.CreateScope(ScopeService.EmailScopeName);

            scopeService.CreateScope(ScopeService.OpenIdScopeName);

            scopeService.CreateScope(ScopeService.ProfileScopeName);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
