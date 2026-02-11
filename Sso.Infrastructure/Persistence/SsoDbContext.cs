using Microsoft.EntityFrameworkCore;
using Sso.Infrastructure.Persistence.Configurations;

namespace Sso.Infrastructure.Persistence
{
    public class SsoDbContext(DbContextOptions<SsoDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ClientPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConsentConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorizationCodeConfiguration());
            modelBuilder.ApplyConfiguration(new ScopeConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        }
    }
}
