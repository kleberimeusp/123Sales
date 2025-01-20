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
            if (request == null || string.IsNullOrWhiteSpace(request.CustomerName))
            {
                return BadRequest(new { Message = "Invalid event data." });
            }

            var purchaseEvent = new PurchaseEvent(Guid.NewGuid(), request.CustomerName, request.TotalAmount);

            _logger.LogInformation("Publishing event for customer: {CustomerName}", request.CustomerName);

            await _eventPublisher.PublishAsync(purchaseEvent);

            return Ok(new { Message = "Event published successfully", Event = purchaseEvent });
        }
    }
}
