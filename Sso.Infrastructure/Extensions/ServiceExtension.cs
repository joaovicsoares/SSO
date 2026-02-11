using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sso.Infrastructure.Persistence;

namespace Sso.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        extension(IServiceCollection serviceCollection)
        {
            public IServiceCollection AddInfrastructure()
            {
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
                }

                return serviceProvider;
            }
        }
    }
}
