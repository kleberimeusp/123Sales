using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs;
using Sales.Application.Events;
using Sales.Domain.Events;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventPublisher eventPublisher, ILogger<EventController> logger)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> PublishEvent([FromBody] EventRequest request)
        {
            if (!IsValidEventRequest(request))
            {
                LogWarning("Invalid event data received.");
                return BadRequest(new { Message = "Invalid event data." });
            }

            var purchaseEvent = CreatePurchaseEvent(request);

            if (!await PublishEventAsync(purchaseEvent, request.CustomerName))
            {
                return StatusCode(500, new { Message = "Failed to publish event." });
            }

            return Ok(new { Message = "Event published successfully", Event = purchaseEvent });
        }

        // PRIVATE METHODS
        private bool IsValidEventRequest(EventRequest request)
        {
            return request != null && !string.IsNullOrWhiteSpace(request.CustomerName);
        }

        private PurchaseEvent CreatePurchaseEvent(EventRequest request)
        {
            return new PurchaseEvent(Guid.NewGuid(), request.CustomerName, request.TotalAmount);
        }

        private async Task<bool> PublishEventAsync(PurchaseEvent purchaseEvent, string customerName)
        {
            try
            {
                LogInformation($"Publishing event for customer: {customerName}");
                await _eventPublisher.PublishAsync(purchaseEvent);
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Error publishing event for customer {customerName}: {ex.Message}");
                return false;
            }
        }

        private void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        private void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        private void LogError(string message)
        {
            _logger.LogError(message);
        }
    }
}
