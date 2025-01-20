using Moq;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Tests.Mocks
{
    public static class MockSaleRepository
    {
        public static Mock<ISaleRepository> GetSaleRepository()
        {
            var mockRepo = new Mock<ISaleRepository>();

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = 1001,
                SaleDate = DateTime.UtcNow,
                Customer = "John Doe",
                TotalSaleValue = 500,
                Branch = "New York",
                Items = new List<SaleItem>
                {
                    new SaleItem { Id = Guid.NewGuid(), SaleId = Guid.NewGuid(), Product = "Laptop", Quantity = 2, UnitPrice = 250 }
                }
            };

            var salesList = new List<Sale> { sale };

            // ✅ Mock GetAll() - Returns a Task<List<Sale>>
            mockRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(salesList);

            // ✅ Mock GetById() - Returns a Task<Sale>
            mockRepo.Setup(repo => repo.GetById(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => salesList.FirstOrDefault(s => s.Id == id));

            // ✅ Mock Add() - Returns a Task<Sale>
            mockRepo.Setup(repo => repo.Add(It.IsAny<Sale>()))
                .ReturnsAsync((Sale s) =>
                {
                    s.Id = Guid.NewGuid();
                    foreach (var item in s.Items)
                    {
                        item.Id = Guid.NewGuid();
                        item.SaleId = s.Id;
                    }
                    salesList.Add(s);
                    return s;
                });

            // ✅ Mock Update() - Returns a Task<Sale>
            mockRepo.Setup(repo => repo.Update(It.IsAny<Sale>()))
                .ReturnsAsync((Sale s) =>
                {
                    var existingSale = salesList.FirstOrDefault(x => x.Id == s.Id);
                    if (existingSale != null)
                    {
                        existingSale.Customer = s.Customer;
                        existingSale.TotalSaleValue = s.TotalSaleValue;
                        existingSale.IsCanceled = s.IsCanceled;
                        existingSale.Items = s.Items;
                        return existingSale;
                    }
                    return s; // Returning the new sale if it doesn't exist in the list
                });

            // ✅ Mock Delete() - Returns a Task<bool>
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) =>
                {
                    var saleToRemove = salesList.FirstOrDefault(s => s.Id == id);
                    if (saleToRemove != null)
                    {
                        salesList.Remove(saleToRemove);
                        return true;
                    }
                    return false;
                });

            return mockRepo;
        }
    }
}
