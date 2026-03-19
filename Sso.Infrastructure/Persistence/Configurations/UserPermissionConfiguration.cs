using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.HasKey(x => new { x.UserId, x.PermissionId });

            builder.HasOne(x => x.User).WithMany(x => x.UserPermissions).HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId);
        }
    }
}
