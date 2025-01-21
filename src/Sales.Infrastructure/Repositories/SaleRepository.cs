using Microsoft.EntityFrameworkCore;
using Sales.Domain.Models;
using Sales.Domain.Interfaces;
using Sales.Infrastructure.Data;

namespace Sales.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAll()
        {
            return (IEnumerable<Sale>)await _context.Sales.Include(s => s.Items).ToListAsync();
        }

        public async Task<Sale?> GetById(Guid id)
        {
            return await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Add(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var sale = await GetById(id);
            if (sale == null) return;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }
}
