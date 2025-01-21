using Moq;
using Sales.Domain.Interfaces;
using Sales.Domain.Models;

namespace Sales.Tests.Mocks
{
    public static class MockSaleRepository
    {
        public static Mock<ISaleRepository> GetSaleRepository()
        {
            var mockRepo = new Mock<ISaleRepository>();
            var salesList = GenerateMockSales();

            SetupGetAll(mockRepo, salesList);
            SetupGetById(mockRepo, salesList);
            SetupAdd(mockRepo, salesList);
            SetupUpdate(mockRepo, salesList);
            SetupDelete(mockRepo, salesList);

            return mockRepo;
        }

        private static List<Sale> GenerateMockSales()
        {
            return new List<Sale>
            {
                new Sale
                {
                    Id = Guid.NewGuid(),
                    SaleNumber = 1001,
                    SaleDate = DateTime.UtcNow,
                    Customer = "John Doe",
                    TotalSaleValue = 500,
                    Branch = "New York",
                    Items = new List<SaleItem>
                    {
                        new SaleItem { Id = Guid.NewGuid(), SaleId = Guid.NewGuid(), ProductName = "Laptop", Quantity = 2, UnitPrice = 250 }
                    }
                }
            };
        }

        private static void SetupGetAll(Mock<ISaleRepository> mockRepo, List<Sale> salesList)
        {
            mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(salesList);
        }

        private static void SetupGetById(Mock<ISaleRepository> mockRepo, List<Sale> salesList)
        {
            mockRepo.Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => salesList.FirstOrDefault(s => s.Id == id));
        }

        private static void SetupAdd(Mock<ISaleRepository> mockRepo, List<Sale> salesList)
        {
            mockRepo.Setup(repo => repo.Add(It.IsAny<Sale>()))
                .Callback<Sale>(s =>
                {
                    s.Id = Guid.NewGuid();
                    foreach (var item in s.Items)
                    {
                        item.Id = Guid.NewGuid();
                        item.SaleId = s.Id;
                    }
                    salesList.Add(s);
                })
                .Returns(Task.CompletedTask);
        }

        private static void SetupUpdate(Mock<ISaleRepository> mockRepo, List<Sale> salesList)
        {
            mockRepo.Setup(repo => repo.Update(It.IsAny<Sale>()))
                .Callback<Sale>(s =>
                {
                    var existingSale = salesList.FirstOrDefault(x => x.Id == s.Id);
                    if (existingSale != null)
                    {
                        existingSale.Customer = s.Customer;
                        existingSale.TotalSaleValue = s.TotalSaleValue;
                        existingSale.IsCanceled = s.IsCanceled;
                        existingSale.Items = s.Items;
                    }
                })
                .Returns(Task.CompletedTask);
        }

        private static void SetupDelete(Mock<ISaleRepository> mockRepo, List<Sale> salesList)
        {
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Guid>()))
                .Returns<Guid>(id =>
                {
                    var saleToRemove = salesList.FirstOrDefault(s => s.Id == id);
                    if (saleToRemove != null)
                    {
                        salesList.Remove(saleToRemove);
                        return Task.FromResult(true);
                    }
                    return Task.FromResult(false);
                });
        }
    }
}
