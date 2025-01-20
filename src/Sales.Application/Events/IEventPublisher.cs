using Sales.Domain.Events;

namespace Sales.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(PurchaseEvent purchaseEvent);
    }
}
