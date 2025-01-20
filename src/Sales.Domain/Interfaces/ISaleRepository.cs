using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sales.API.Domain.Entities;

namespace Sales.API.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> GetById(Guid id);
        Task<IEnumerable<Sale>> GetAll();
        Task Add(Sale sale);
        Task Update(Sale sale);
        Task Remove(Guid id);
    }
}
