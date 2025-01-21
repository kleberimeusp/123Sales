using Microsoft.EntityFrameworkCore;
using Sales.Domain.Models;

public class SalesDBContext : DbContext
{
    public SalesDBContext(DbContextOptions<SalesDBContext> options)
        : base(options) { }

    public DbSet<Sale> Sales { get; set; }
}


