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

            builder.HasAlternateKey(x => x.Guid);

            builder.HasOne(x => x.User).WithMany();

            builder.HasOne(x => x.Client).WithMany();
        }
    }
}
