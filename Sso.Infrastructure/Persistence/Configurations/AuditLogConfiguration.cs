using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sso.Domain.Entities;

namespace Sso.Infrastructure.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Client).WithMany().HasForeignKey(x => x.ClientId);

            builder.Property(x => x.EventType).HasConversion<string>();

            builder.OwnsOne(x => x.Data, builder => { builder.ToJson(); });
        }
    }
}
