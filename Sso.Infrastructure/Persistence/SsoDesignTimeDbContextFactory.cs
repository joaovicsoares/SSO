using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Sso.Infrastructure.Persistence
{
    public class DesignTimeSsoDbContextFactory : IDesignTimeDbContextFactory<SsoDbContext>
    {
        public SsoDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SsoDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new SsoDbContext(optionsBuilder.Options);
        }
    }
}
