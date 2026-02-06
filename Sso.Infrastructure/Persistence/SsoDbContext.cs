using Microsoft.EntityFrameworkCore;

namespace Sso.Infrastructure.Persistence
{
    public class SsoDbContext : DbContext
    {
        public SsoDbContext(DbContextOptions<SsoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
