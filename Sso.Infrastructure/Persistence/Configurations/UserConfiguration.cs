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

            builder
                .HasMany(x => x.Roles)
                .WithMany();

            builder
                .HasMany(x => x.Permissions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasIndex(x => x.Email);
        }
    }
}
