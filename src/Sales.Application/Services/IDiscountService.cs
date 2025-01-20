using Sales.Domain.Entities;

namespace Sales.Application.Services
{
    public interface IDiscountService
    {
        decimal ApplyDiscount(decimal originalPrice, decimal discountPercentage);
        decimal ApplyBulkDiscount(decimal totalAmount, int quantity);
        decimal ApplyFixedDiscount(decimal originalPrice, decimal discountAmount);
        decimal ApplyDiscount(Sale sale);

        decimal ApplyPercentageDiscount(decimal originalPrice, decimal discountPercentage);

    }
}
