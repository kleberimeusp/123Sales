namespace Sales.Application.DTOs
{
    public class SaleDto
    {
        public Guid Id { get; set; }

        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalSaleValue { get; set; }
        public string Branch { get; set; }
        public List<SaleItemDto> Items { get; set; }
        public bool IsCanceled { get; set; }
    }

    public class SaleItemDto
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemValue => (UnitPrice * Quantity) - Discount;
    }
}
