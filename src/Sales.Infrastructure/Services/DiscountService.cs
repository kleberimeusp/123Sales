using Sales.Application.Services;
using Sales.Domain.Models;

namespace Sales.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        public decimal ApplyDiscount(decimal originalPrice, decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100.");

            return originalPrice - (originalPrice * (discountPercentage / 100));
        }

        public decimal ApplyBulkDiscount(decimal totalAmount, int quantity)
        {
            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 identical items.");

            if (quantity < 4)
                return totalAmount; 

            decimal discountPercentage = quantity >= 10 ? 0.20m : 0.10m;
            return totalAmount - (totalAmount * discountPercentage);
        }

        public decimal ApplyFixedDiscount(decimal originalPrice, decimal discountAmount)
        {
            if (discountAmount < 0 || discountAmount > originalPrice)
                throw new ArgumentException("Invalid discount amount.");

            return originalPrice - discountAmount;
        }

        public decimal ApplyDiscount(Sale sale)
        {
            if (sale == null || sale.Items == null || sale.Items.Count == 0)
                throw new ArgumentException("Sale must have items to apply discounts.");

            decimal totalDiscountedValue = 0;

            foreach (var item in sale.Items)
            {
                decimal itemTotal = item.UnitPrice * item.Quantity;
                decimal discountedValue = ApplyBulkDiscount(itemTotal, item.Quantity);
                totalDiscountedValue += discountedValue;
            }

            return totalDiscountedValue;
        }

        public decimal ApplyPercentageDiscount(decimal originalPrice, decimal discountPercentage)
        {
            throw new NotImplementedException();
        }
    }
}
