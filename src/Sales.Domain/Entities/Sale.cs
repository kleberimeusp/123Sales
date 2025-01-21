namespace Sales.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }  
        public int SaleNumber { get; set; }  
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalSaleValue { get; set; }
        public string Branch { get; set; }
        public List<SaleItem> Items { get; set; } = new List<SaleItem>(); 
        public bool IsCanceled { get; set; }

        public Sale()
        {
            Id = Guid.NewGuid(); 
        }
    }
}
