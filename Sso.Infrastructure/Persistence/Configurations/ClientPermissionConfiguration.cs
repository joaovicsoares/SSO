using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class ClientPermissionConfiguration : IEntityTypeConfiguration<ClientPermission>
    {
        public void Configure(EntityTypeBuilder<ClientPermission> builder)
        {
            builder.HasKey(x => new { x.ClientId, x.PermissionId });

            builder.HasOne(x => x.Client).WithMany(x => x.ClientPermissions).HasForeignKey(x => x.ClientId);

            builder.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId);
        }
    }
}
