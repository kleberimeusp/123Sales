using Microsoft.AspNetCore.Mvc;
using Sales.Application.Services;
using Sales.Domain.Models;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ILoggingService _loggingService;

        public DiscountController(IDiscountService discountService, ILoggingService loggingService)
        {
            _discountService = discountService;
            _loggingService = loggingService;
        }

        [HttpGet("bulk")]
        public IActionResult ApplyBulkDiscount(decimal unitPrice, int quantity)
        {
            if (unitPrice <= 0 || quantity <= 0)
            {
                _loggingService.LogWarning("Invalid parameters for bulk discount.");
                return BadRequest(new { Error = "Unit price and quantity must be greater than zero." });
            }

            try
            {
                var finalAmount = _discountService.ApplyBulkDiscount(unitPrice, quantity);
                return Ok(new { UnitPrice = unitPrice, Quantity = quantity, FinalAmount = finalAmount });
            }
            catch (ArgumentException ex)
            {
                _loggingService.LogWarning(ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessSale([FromBody] Sale sale)
        {
            if (!IsValidSale(sale))
            {
                _loggingService.LogWarning("Invalid sale data received.");
                return BadRequest(new { Error = "Invalid sale data" });
            }

            try
            {
                var discountedTotal = _discountService.ApplyDiscount(sale);
                _loggingService.LogInformation($"Discount applied successfully to sale ID: {sale.Id}");

                return Ok(new
                {
                    Message = "Sale processed successfully",
                    OriginalTotal = sale.TotalSaleValue,
                    DiscountedTotal = discountedTotal,
                    Items = sale.Items
                });
            }
            catch (InvalidOperationException ex)
            {
                _loggingService.LogError($"Error processing sale ID {sale.Id}: {ex.Message}");
                return BadRequest(new { Error = ex.Message });
            }
        }

        // PRIVATE METHODS
        private bool IsValidSale(Sale sale)
        {
            return sale != null && sale.Items != null && sale.Items.Count > 0;
        }
    }
}
