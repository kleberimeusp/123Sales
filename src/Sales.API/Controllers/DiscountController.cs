using Microsoft.AspNetCore.Mvc;
using Sales.Application.Services;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("bulk")]
        public IActionResult ApplyBulkDiscount(decimal unitPrice, int quantity)
        {
            try
            {
                var finalAmount = _discountService.ApplyBulkDiscount(unitPrice, quantity);
                return Ok(new { UnitPrice = unitPrice, Quantity = quantity, FinalAmount = finalAmount });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
