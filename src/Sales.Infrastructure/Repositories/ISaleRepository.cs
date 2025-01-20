using Sales.Domain.Entities;

namespace Sales.Infrastructure.Repositories
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
