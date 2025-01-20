using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalesAPI.Models;
using SalesAPI.Repositories;
using SalesAPI.Services;

namespace SalesAPI.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly DiscountService _discountService;
        private readonly LoggingService _loggingService;

        public SalesController(
            ISaleRepository saleRepository,
            DiscountService discountService,
            LoggingService loggingService)
        {
            _saleRepository = saleRepository;
            _discountService = discountService;
            _loggingService = loggingService;
        }

        // ✅ GET ALL SALES
        [HttpGet]
        public async Task<IEnumerable<Sale>> Get()
        {
            _loggingService.LogInfo("Fetching all sales records.");
            return await _saleRepository.GetAll();
        }

        // ✅ GET SALE BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> Get(Guid id)
        {
            _loggingService.LogInfo($"Fetching sale with ID: {id}");
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
            {
                _loggingService.LogWarning($"Sale with ID {id} not found.");
                return NotFound();
            }
            return sale;
        }

        // ✅ CREATE SALE
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sale sale)
        {
            if (sale == null || sale.Items == null || sale.Items.Count == 0)
            {
                _loggingService.LogWarning("Invalid sale data received.");
                return BadRequest("Invalid sale data");
            }

            sale.Id = Guid.NewGuid();
            sale.SaleDate = DateTime.UtcNow;
            await _saleRepository.Add(sale);
            _loggingService.LogInfo($"Sale created successfully. Sale ID: {sale.Id}");

            return CreatedAtAction(nameof(Get), new { id = sale.Id }, sale);
        }

        // ✅ UPDATE SALE
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Sale sale)
        {
            _loggingService.LogInfo($"Updating sale with ID: {id}");
            var existingSale = await _saleRepository.GetById(id);
            if (existingSale == null)
            {
                _loggingService.LogWarning($"Sale with ID {id} not found.");
                return NotFound();
            }

            existingSale.CustomerId = sale.CustomerId;
            existingSale.Items = sale.Items;
            existingSale.TotalAmount = sale.TotalAmount;
            existingSale.Canceled = sale.Canceled;

            await _saleRepository.Update(existingSale);
            _loggingService.LogInfo($"Sale with ID {id} updated successfully.");

            return NoContent();
        }

        // ✅ DELETE SALE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _loggingService.LogInfo($"Attempting to delete sale with ID: {id}");
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
            {
                _loggingService.LogWarning($"Sale with ID {id} not found.");
                return NotFound();
            }

            await _saleRepository.Remove(id);
            _loggingService.LogInfo($"Sale with ID {id} deleted successfully.");

            return NoContent();
        }

        // ✅ PROCESS SALE (APPLY DISCOUNT)
        [HttpPost("process")]
        public IActionResult ProcessSale([FromBody] List<SaleItem> items)
        {
            if (items == null || items.Count == 0)
            {
                _loggingService.LogWarning("Invalid sale data received.");
                return BadRequest("Invalid sale data.");
            }

            try
            {
                _discountService.ApplyDiscount(items);
                _loggingService.LogInfo("Discount applied successfully to sale.");
                return Ok(new { Message = "Sale processed successfully", Items = items });
            }
            catch (InvalidOperationException ex)
            {
                _loggingService.LogError($"Error processing sale: {ex.Message}");
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
