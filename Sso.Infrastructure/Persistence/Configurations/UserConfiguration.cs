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

            builder.HasAlternateKey(x => x.Guid);

            builder.HasIndex(x => x.Email);

            builder.HasMany(x => x.Roles).WithMany();

            builder.HasMany(x => x.UserPermissions).WithOne(x => x.User);
        }
    }
}
