using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesAPI.Models;

namespace SalesAPI.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> Add(Sale sale);
        Task<IEnumerable<Sale>> GetAll();
        Task<Sale> GetById(Guid id);
        Task<Sale> Update(Sale sale);
        Task<bool> Delete(Guid id);
    }
}
