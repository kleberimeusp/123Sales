using Sales.Domain.Entities;

namespace Sales.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale?> GetById(Guid id);
        Task<IEnumerable<Sale>> GetAll();
        Task<Sale> Add(Sale sale);
        Task<Sale> Update(Sale sale);
        Task<bool> Delete(Guid id);
    }
}
