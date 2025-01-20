using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Sales.Domain.Entities;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Sales.Tests.IntegrationTests
{
    public class SalesApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SalesApiTests(WebApplicationFactory<Program> factory) // ✅ Use only Program (No Sales.API)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _client = factory.CreateClient();
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
            var sale = new Sale
            {
                SaleNumber = 4004,
                SaleDate = DateTime.UtcNow,
                Customer = "Bob",
                TotalSaleValue = 1200,
                Branch = "Chicago",
                Items = new List<SaleItem> { new SaleItem { Product = "Tablet", Quantity = 1, UnitPrice = 1200 } }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(sale), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/sales", jsonContent);

            response.EnsureSuccessStatusCode();
        }
    }
}
