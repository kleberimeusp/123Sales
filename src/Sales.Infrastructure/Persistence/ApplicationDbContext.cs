using Microsoft.EntityFrameworkCore;
using Sales.Domain.Models;

namespace Sales.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }  // Se houver a classe SaleItem

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => s.Id);  // Define a chave primária
                entity.Property(s => s.SaleNumber).IsRequired();
                entity.Property(s => s.Customer).HasMaxLength(255);
                entity.Property(s => s.TotalSaleValue).HasColumnType("decimal(18,2)");
                entity.Property(s => s.Branch).HasMaxLength(100);
                entity.HasMany(s => s.Items).WithOne().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
