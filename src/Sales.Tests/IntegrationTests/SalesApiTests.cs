using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Sales.Domain.Models;
using Xunit;

namespace Sales.Tests.IntegrationTests
{
    public class SalesApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SalesApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory?.CreateClient() ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task Get_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/sales");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_CreatesSaleSuccessfully()
        {
            var sale = CreateTestSale();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(sale), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/sales", jsonContent);

            response.EnsureSuccessStatusCode();
        }

        private Sale CreateTestSale()
        {
            return new Sale
            {
                SaleNumber = 4004,
                SaleDate = DateTime.UtcNow,
                Customer = "Bob",
                TotalSaleValue = 1200,
                Branch = "Chicago",
                Items = new List<SaleItem> { new SaleItem { ProductName = "Tablet", Quantity = 1, UnitPrice = 1200 } }
            };
        }
    }
}
