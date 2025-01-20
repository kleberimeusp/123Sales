using Microsoft.AspNetCore.Mvc;
using Sales.Domain.Interfaces;
using Sales.Application.Services;
using Sales.Domain.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IDiscountService _discountService;
        private readonly ILoggingService _loggingService;

        public SalesController(
            ISaleRepository saleRepository,
            IDiscountService discountService,
            ILoggingService loggingService)
        {
            _saleRepository = saleRepository;
            _discountService = discountService;
            _loggingService = loggingService;
        }

        // ✅ GET ALL SALES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> Get()
        {
            _loggingService.LogInformation("Fetching all sales records.");
            var sales = await _saleRepository.GetAll();
            return Ok(sales);
        }

        // ✅ GET SALE BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> Get(Guid id)
        {
            _loggingService.LogInformation($"Fetching sale with ID: {id}");
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
            {
                _loggingService.LogWarning($"Sale with ID {id} not found.");
                return NotFound(new { Message = "Sale not found" });
            }
            return Ok(sale);
        }

        // ✅ CREATE SALE
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Sale sale)
        {
            if (sale == null || sale.Items == null || sale.Items.Count == 0)
            {
                _loggingService.LogWarning("Invalid sale data received.");
                return BadRequest(new { Error = "Invalid sale data" });
            }

            sale.Id = Guid.NewGuid();
            sale.SaleDate = DateTime.UtcNow;
            await _saleRepository.Add(sale);
            _loggingService.LogInformation($"Sale created successfully. Sale ID: {sale.Id}");

            return CreatedAtAction(nameof(Get), new { id = sale.Id }, sale);
        }

        // ✅ UPDATE SALE
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Sale sale)
        {
            _loggingService.LogInformation($"Updating sale with ID: {id}");
            var existingSale = await _saleRepository.GetById(id);
            if (existingSale == null)
            {
                _loggingService.LogWarning($"Sale with ID {id} not found.");
                return NotFound(new { Message = "Sale not found" });
            }

            existingSale.Items = sale.Items;
            existingSale.TotalSaleValue = sale.TotalSaleValue;
            existingSale.IsCanceled = sale.IsCanceled;

            await _saleRepository.Update(existingSale);
            _loggingService.LogInformation($"Sale with ID {id} updated successfully.");

            return NoContent();
        }

        // ✅ DELETE SALE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _loggingService.LogInformation($"Attempting to delete sale with ID: {id}");
            var sale = await _saleRepository.GetById(id);
            if (sale == null)
            {
                _loggingService.LogWarning($"Sale with ID {id} not found.");
                return NotFound(new { Message = "Sale not found" });
            }

            await _saleRepository.Delete(id);
            _loggingService.LogInformation($"Sale with ID {id} deleted successfully.");

            return NoContent();
        }

        // ✅ PROCESS SALE (APPLY DISCOUNT)
        [HttpPost("process")]
        public IActionResult ProcessSale([FromBody] Sale sale)
        {
            if (sale == null || sale.Items == null || sale.Items.Count == 0)
            {
                _loggingService.LogWarning("Invalid sale data received.");
                return BadRequest(new { Error = "Invalid sale data" });
            }

            try
            {
                decimal discountedTotal = _discountService.ApplyDiscount(sale);
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
    }
}
