using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sales.Infrastructure.Persistence
{
    public class SalesDBContextFactory : IDesignTimeDbContextFactory<SalesDBContext>
    {
        public SalesDBContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
     
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("SalesDB");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'SalesDB' is not configured.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<SalesDBContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new SalesDBContext(optionsBuilder.Options);
        }
    }
}
