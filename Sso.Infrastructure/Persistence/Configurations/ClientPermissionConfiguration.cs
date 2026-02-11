using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class ClientPermissionConfiguration : IEntityTypeConfiguration<ClientPermission>
    {
        public void Configure(EntityTypeBuilder<ClientPermission> builder)
        {
        }
    }
}
