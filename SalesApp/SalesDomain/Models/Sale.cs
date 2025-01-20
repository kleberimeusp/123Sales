using System;
using System.Collections.Generic;

namespace SalesAPI.Models
{
    public class Sale
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerId { get; set; }
        public string Branch { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Canceled { get; set; }
        public List<SaleItem> Items { get; set; }
    }
}
