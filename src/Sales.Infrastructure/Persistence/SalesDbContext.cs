using Microsoft.EntityFrameworkCore;
using Sales.Domain.Models;

namespace Sales.Infrastructure.Persistence
{
    public class SalesDBContext : DbContext
    {
        public SalesDBContext(DbContextOptions<SalesDBContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("The ConnectionString property has not been initialized.");
            }
        }
    }
}
