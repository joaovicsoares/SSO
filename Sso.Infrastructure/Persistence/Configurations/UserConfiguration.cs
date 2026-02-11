using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Guid).IsUnique();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasMany(x => x.Roles).WithMany();

            builder.HasMany(x => x.UserPermissions).WithOne(x => x.User);
        }
    }
}
