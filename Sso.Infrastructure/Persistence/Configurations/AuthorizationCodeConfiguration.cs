using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class AuthorizationCodeConfiguration : IEntityTypeConfiguration<AuthorizationCode>
    {
        public void Configure(EntityTypeBuilder<AuthorizationCode> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Guid).IsUnique();

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Client).WithMany().HasForeignKey(x => x.ClientId);
        }
    }
}
