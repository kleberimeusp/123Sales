using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces;
using Sales.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.Sales.Include(s => s.Items).ToListAsync();
        }

        public async Task<Sale?> GetById(Guid id)
        {
            return await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> Add(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sale> Update(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<bool> Delete(Guid id)
        {
            var sale = await GetById(id);
            if (sale == null) return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
