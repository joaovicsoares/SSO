using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sso.Application.Persistence;
using Sso.Domain.Repositories;
using Sso.Domain.Services;
using Sso.Infrastructure.Persistence;
using Sso.Infrastructure.Persistence.Repositories;
using Sso.Infrastructure.Persistence.Seeds;

namespace Sso.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        extension(IServiceCollection serviceCollection)
        {
            public IServiceCollection AddInfrastructure()
            {
                serviceCollection.AddScoped<ScopeService>();

                serviceCollection.AddScoped<IScopeRepository, ScopeRepository>();

                serviceCollection.AddScoped<IAuditLogRepository, AuditLogRepository>();

                serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

                serviceCollection.AddScoped<ScopeSeed>();

                return serviceCollection;
            }
        }

        extension(IServiceProvider serviceProvider)
        {
            public async Task<IServiceProvider> InitInfrastructureAsync()
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<SsoDbContext>();
                    await db.Database.MigrateAsync();

                    var scopeSeed = scope.ServiceProvider.GetRequiredService<ScopeSeed>();
                    await scopeSeed.SeedAsync();
                }

                return serviceProvider;
            }
        }
    }
}
