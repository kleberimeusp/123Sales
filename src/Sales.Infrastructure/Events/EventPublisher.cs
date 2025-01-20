using Sales.Application.Events;
using Sales.Domain.Events;
using Serilog;

namespace Sales.Infrastructure.Events
{
    public class EventPublisher : IEventPublisher
    {
        public async Task PublishAsync(PurchaseEvent purchaseEvent)
        {
            if (purchaseEvent == null)
            {
                Log.Warning("Attempted to publish a null event.");
                return;
            }

            // Log event publishing
            Log.Information("Event Published: PurchaseId={PurchaseId}, Customer={CustomerName}, TotalAmount={TotalAmount}",
                purchaseEvent.PurchaseId, purchaseEvent.CustomerName ?? "Unknown", purchaseEvent.TotalAmount);

            await Task.CompletedTask;
        }
    }
}
