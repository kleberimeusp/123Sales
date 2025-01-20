using System;

namespace Sales.API.Models
{
    public class SaleItem
    {
        public string ProductId { get; set; }  // External identity reference for product
        public int Quantity { get; set; }  // Quantity of the product
        public decimal UnitPrice { get; set; }  // Price per unit

        // Apply discount based on quantity rules
        public decimal Discount
        {
            get
            {
                if (Quantity > 20)
                    throw new InvalidOperationException($"Cannot sell more than 20 units of product {ProductId}.");
                if (Quantity < 4)
                    return 0m; // No discount for less than 4 items
                if (Quantity >= 10 && Quantity <= 20)
                    return (UnitPrice * Quantity) * 0.20m; // 20% discount for 10-20 items
                if (Quantity >= 5)
                    return (UnitPrice * Quantity) * 0.10m; // 10% discount for 5-9 items
                return 0m;
            }
        }

        public decimal TotalItemValue => (UnitPrice * Quantity) - Discount;  // Final price after discount
    }
}
