using Microsoft.EntityFrameworkCore;
using Sso.Domain.Entities;
using Sso.Domain.ValueObjects;
using Sso.Infrastructure.Persistence.Configurations;
using Sso.Infrastructure.Persistence.Converters;

namespace Sso.Infrastructure.Persistence
{
    public class SsoDbContext(DbContextOptions<SsoDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<UserConsent> UserConsents { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientPermission> ClientPermissions { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Scope> Scopes { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<AuthorizationCode> AuthorizationCodes { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<Email>()
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
