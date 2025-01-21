using Microsoft.AspNetCore.Mvc;
using Moq;
using Sales.API.Controllers;
using Sales.Application.Services;
using Sales.Domain.Models;
using Xunit;

namespace Sales.Tests.UnitTests
{
    public class DiscountControllerTests
    {
        private readonly DiscountController _controller;
        private readonly Mock<IDiscountService> _mockDiscountService;
        private readonly Mock<ILoggingService> _mockLoggingService;

        public DiscountControllerTests()
        {
            _mockDiscountService = new Mock<IDiscountService>();
            _mockLoggingService = new Mock<ILoggingService>();

            _controller = new DiscountController(
                _mockDiscountService.Object,
                _mockLoggingService.Object
            );
        }

        [Fact]
        public async Task ProcessSale_AppliesDiscountSuccessfully()
        {
            var sale = CreateTestSale();
            _mockDiscountService.Setup(service => service.ApplyDiscount(It.IsAny<Sale>()))
                .Returns(sale.TotalSaleValue * 0.9m);

            var result = _controller.ProcessSale(sale);
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("processed successfully", actionResult.Value.ToString());
        }

        [Fact]
        public void ApplyBulkDiscount_ReturnsCorrectAmount()
        {
            decimal unitPrice = 100;
            int quantity = 5;
            decimal expectedDiscountedAmount = 450;

            _mockDiscountService.Setup(service => service.ApplyBulkDiscount(unitPrice, quantity))
                .Returns(expectedDiscountedAmount);

            var result = _controller.ApplyBulkDiscount(unitPrice, quantity);
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedDiscountedAmount, ((dynamic)actionResult.Value).FinalAmount);
        }

        [Fact]
        public void ApplyBulkDiscount_ReturnsBadRequest_ForInvalidInput()
        {
            var result = _controller.ApplyBulkDiscount(-100, -5);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private Sale CreateTestSale()
        {
            return new Sale
            {
                SaleNumber = 3003,
                SaleDate = DateTime.UtcNow,
                Customer = "Alice",
                TotalSaleValue = 1000,
                Branch = "San Francisco",
                Items = new List<SaleItem> { new SaleItem { ProductName = "Monitor", Quantity = 2, UnitPrice = 500 } }
            };
        }
    }
}
