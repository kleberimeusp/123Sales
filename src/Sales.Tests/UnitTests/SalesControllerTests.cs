using Microsoft.AspNetCore.Mvc;
using Moq;
using Sales.API.Controllers;
using Sales.Application.Services;
using Sales.Domain.Interfaces;
using Sales.Domain.Models;
using Sales.Tests.Mocks;
using Xunit;

namespace Sales.Tests.UnitTests
{
    public class SalesControllerTests
    {
        private readonly SalesController _controller;
        private readonly Mock<ISaleRepository> _mockSaleRepo;
        private readonly Mock<IDiscountService> _mockDiscountService;
        private readonly Mock<ILoggingService> _mockLoggingService;

        public SalesControllerTests()
        {
            _mockSaleRepo = MockSaleRepository.GetSaleRepository();
            _mockDiscountService = MockDiscountService.GetDiscountService();
            _mockLoggingService = MockLoggingService.GetLoggingService();

            _controller = new SalesController(
                _mockSaleRepo.Object,
                _mockDiscountService.Object,
                _mockLoggingService.Object
            );
        }

        [Fact]
        public async Task Get_ReturnsAllSales()
        {
            var result = await _controller.GetAll();
            Assert.IsType<ActionResult<IEnumerable<Sale>>>(result);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenSaleDoesNotExist()
        {
            var result = await _controller.GetById(Guid.NewGuid());
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Post_CreatesSaleSuccessfully()
        {
            var sale = CreateTestSale();
            var result = await _controller.Create(sale);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task ProcessSale_AppliesDiscountSuccessfully()
        {
            var sale = CreateTestSale();
            var result = await _controller.ProcessSale(sale);
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("processed successfully", actionResult.Value.ToString());
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
