namespace Sales.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }  // Unique Identifier for each item
        public Guid SaleId { get; set; }  // Foreign Key reference to `Sale`
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemValue => (UnitPrice * Quantity) - Discount;

        public SaleItem()
        {
            Id = Guid.NewGuid(); // Automatically generates a unique ID when creating an item
        }
    }
}
