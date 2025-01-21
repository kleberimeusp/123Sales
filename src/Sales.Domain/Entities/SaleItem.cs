namespace Sales.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }  
        public Guid SaleId { get; set; }  
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemValue => (UnitPrice * Quantity) - Discount;

        public SaleItem()
        {
            Id = Guid.NewGuid(); 
        }
    }
}
