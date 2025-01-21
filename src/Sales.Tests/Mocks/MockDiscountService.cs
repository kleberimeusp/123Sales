using Moq;
using Sales.Application.Services;
using Sales.Domain.Models;

namespace Sales.Tests.Mocks
{
    public static class MockDiscountService
    {
        public static Mock<IDiscountService> GetDiscountService()
        {
            var mockService = new Mock<IDiscountService>();

            mockService.Setup(service => service.ApplyDiscount(It.IsAny<Sale>()))
                .Returns((Sale sale) => sale.TotalSaleValue * 0.9m); // 10% Discount

            return mockService;
        }
    }
}
