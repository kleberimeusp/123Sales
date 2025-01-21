using Sales.Domain.Models;

namespace Sales.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAll();
        Task<Sale?> GetById(Guid id);
        Task Add(Sale sale);
        Task Update(Sale sale);
        Task Delete(Guid id);
    }
}
