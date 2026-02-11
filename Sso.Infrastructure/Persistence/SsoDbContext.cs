using Microsoft.EntityFrameworkCore;
using Sso.Domain.ValueObjects;
using Sso.Infrastructure.Persistence.Configurations;
using Sso.Infrastructure.Persistence.Converters;

namespace Sso.Infrastructure.Persistence
{
    public class SsoDbContext(DbContextOptions<SsoDbContext> options) : DbContext(options)
    {
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<Email>()
                .HaveMaxLength(Email.MaxLength)
                .HaveConversion<EmailStringConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConsentConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new ClientPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorizationCodeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ScopeConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        }
    }
}
