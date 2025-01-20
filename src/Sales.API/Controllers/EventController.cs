using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sales.API.Events;
using Sales.API.Services;
using System;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;

        public EventController(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        [HttpPost("{eventType}")]
        public IActionResult PublishEvent(string eventType)
        {
            var purchaseId = Guid.NewGuid(); // Simulating a purchase ID
            var purchaseEvent = new PurchaseEvent(eventType, purchaseId);

            _eventPublisher.PublishEvent(purchaseEvent);

            return Ok(new { Message = "Event published successfully", purchaseEvent });
        }
    }
}
