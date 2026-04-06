using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sso.Application.Authentication;
using Sso.Application.Persistence;
using Sso.Domain.Repositories;
using Sso.Domain.Services;
using Sso.Infrastructure.Persistence;
using Sso.Infrastructure.Persistence.Repositories;
using Sso.Infrastructure.Persistence.Seeds;
using Sso.Infrastructure.Security;

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

                serviceCollection.AddScoped<IUserRepository, UserRepository>();

                //serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();

                serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

                serviceCollection.AddScoped<ScopeSeed>();

                serviceCollection.AddScoped<UserSeed>();

            // Register password hasher
            serviceCollection.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            // Register JWT service
            serviceCollection.AddScoped<IJwtService, JwtService>();

            // Register OAuth repositories
            serviceCollection.AddScoped<IAuthorizationCodeRepository, AuthorizationCodeRepository>();
            serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Register OAuth service
            serviceCollection.AddScoped<IOAuthService, OAuthService>();

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

                    var userSeed = scope.ServiceProvider.GetRequiredService<UserSeed>();
                    await userSeed.SeedAsync();
            }

                return serviceProvider;
            }
        }
    }
}
