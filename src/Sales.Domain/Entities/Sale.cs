namespace Sales.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }  // Unique Identifier
        public int SaleNumber { get; set; }  // Sale number (for display)
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalSaleValue { get; set; }
        public string Branch { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>(); // Ensuring it's not null
        public bool IsCanceled { get; set; }

        public Sale()
        {
            Id = Guid.NewGuid(); // Automatically generates a unique ID when creating a sale
        }
    }
}
