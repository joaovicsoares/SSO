using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasAlternateKey(x => x.Guid);

            builder.HasIndex(x => x.ClientId);

            builder.HasMany(x => x.ClientPermissions).WithOne(x => x.Client);

            builder.HasMany(x => x.Scopes).WithOne();
        }
    }
}
