using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sales.API.Controllers;
using Sales.Application.Services;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces;
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
            var result = await _controller.Get();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Sale>>>(result);
            Assert.NotEmpty((IEnumerable<Sale>)actionResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenSaleDoesNotExist()
        {
            var result = await _controller.Get(Guid.NewGuid());

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task Post_CreatesSaleSuccessfully()
        {
            var sale = new Sale
            {
                SaleNumber = 2002,
                SaleDate = DateTime.UtcNow,
                Customer = "Jane Doe",
                TotalSaleValue = 600,
                Branch = "Los Angeles",
                Items = new List<SaleItem> { new SaleItem { Product = "Phone", Quantity = 1, UnitPrice = 600 } }
            };

            var result = await _controller.Post(sale);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task ProcessSale_AppliesDiscountSuccessfully()
        {
            var sale = new Sale
            {
                SaleNumber = 3003,
                SaleDate = DateTime.UtcNow,
                Customer = "Alice",
                TotalSaleValue = 1000,
                Branch = "San Francisco",
                Items = new List<SaleItem> { new SaleItem { Product = "Monitor", Quantity = 2, UnitPrice = 500 } }
            };

            var result = _controller.ProcessSale(sale);
            var actionResult = Assert.IsType<OkObjectResult>(result);

            Assert.Contains("processed successfully", actionResult.Value.ToString());
        }
    }
}
