namespace Sales.Application.DTOs
{
    public class SaleDTO
    {
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalSaleValue { get; set; }
        public string Branch { get; set; }
        public List<SaleItemDTO> Items { get; set; }
        public bool IsCanceled { get; set; }
    }

    public class SaleItemDTO
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemValue => (UnitPrice * Quantity) - Discount;
    }
}
