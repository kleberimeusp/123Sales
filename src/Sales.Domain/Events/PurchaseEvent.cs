namespace Sales.Domain.Events
{
    public class PurchaseEvent
    {
        public Guid PurchaseId { get; }
        public string CustomerName { get; }
        public decimal TotalAmount { get; }
        public DateTime Timestamp { get; }

        public PurchaseEvent(Guid purchaseId, string customerName, decimal totalAmount = 0)
        {
            PurchaseId = purchaseId;
            CustomerName = customerName ?? "Unknown";
            TotalAmount = totalAmount;
            Timestamp = DateTime.UtcNow;
        }
    }
}
