using Microsoft.AspNetCore.Mvc;
using Sales.Domain.Interfaces;
using Sales.Application.DTOs;
using Sales.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Sales.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILoggingService _loggingService;

        public SalesController(ISaleRepository saleRepository, ILoggingService loggingService)
        {
            _saleRepository = saleRepository;
            _loggingService = loggingService;
        }

        // GET ALL SALES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll()
        {
            LogInfo("Fetching all sales records.");

            var sales = await _saleRepository.GetAll();
            var salesDto = sales.Select(MapToDto).ToList();

            return Ok(salesDto);
        }

        // GET SALE BY ID
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SaleDto>> GetById(Guid id)
        {
            LogInfo($"Fetching sale with ID: {id}");

            var sale = await FindSaleById(id);
            if (sale == null) return NotFound(new { Message = "Sale not found" });

            return Ok(MapToDto(sale));
        }

        // CREATE SALE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Sale sale)
        {
            if (!IsValidSale(sale)) return BadRequest(new { Error = "Invalid sale data." });

            PrepareNewSale(sale);
            await _saleRepository.Add(sale);

            LogInfo($"Sale created successfully. Sale ID: {sale.Id}");
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, MapToDto(sale));
        }

        // UPDATE SALE
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Sale sale)
        {
            var existingSale = await FindSaleById(id);
            if (existingSale == null)
            {
                return NotFound(new { Message = "Sale not found" });
            }

            UpdateSaleDetails(existingSale, sale);
            await _saleRepository.Update(existingSale);

            LogInfo($"Sale with ID {id} updated successfully.");
            return NoContent();
        }

        // DELETE SALE
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            LogInfo($"Attempting to delete sale with ID: {id}");

            var sale = await FindSaleById(id);
            if (sale == null) return NotFound(new { Message = "Sale not found" });

            await _saleRepository.Delete(id);

            LogInfo($"Sale with ID {id} deleted successfully.");
            return NoContent();
        }

        // PRIVATE METHODS TO ENFORCE DRY PRINCIPLE
        private async Task<Sale> FindSaleById(Guid id)
        {
            return await _saleRepository.GetById(id);
        }

        private bool IsValidSale(Sale sale)
        {
            return sale != null && sale.Items != null && sale.Items.Any();
        }

        private void PrepareNewSale(Sale sale)
        {
            sale.Id = Guid.NewGuid();
            sale.SaleDate = DateTime.UtcNow;

            foreach (var item in sale.Items)
            {
                item.Id = Guid.NewGuid();
                item.SaleId = sale.Id;
            }
        }

        private void UpdateSaleDetails(Sale existingSale, Sale updatedSale)
        {
            existingSale.SaleNumber = updatedSale.SaleNumber;
            existingSale.Customer = updatedSale.Customer;
            existingSale.TotalSaleValue = updatedSale.TotalSaleValue;
            existingSale.Branch = updatedSale.Branch;
            existingSale.IsCanceled = updatedSale.IsCanceled;
            existingSale.Items = updatedSale.Items;
        }

        private SaleDto MapToDto(Sale sale)
        {
            return new SaleDto
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                Customer = sale.Customer,
                TotalSaleValue = sale.TotalSaleValue,
                Branch = sale.Branch,
                IsCanceled = sale.IsCanceled,
                Items = sale.Items.Select(item => new SaleItemDto
                {
                    Id = item.Id,
                    SaleId = item.SaleId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = 0 // Set discount logic here if needed
                }).ToList()
            };
        }

        private void LogInfo(string message)
        {
            _loggingService.LogInformation(message);
        }
    }
}
