using Microsoft.EntityFrameworkCore;
using Sales.Domain.Models;

public class AnotherDbContext : DbContext
{
    public AnotherDbContext(DbContextOptions<AnotherDbContext> options)
        : base(options) { }

    public DbSet<AnotherEntity> AnotherEntities { get; set; }
}
