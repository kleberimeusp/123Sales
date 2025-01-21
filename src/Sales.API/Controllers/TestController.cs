using Microsoft.AspNetCore.Mvc;
using Sales.Application.Services;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly ILoggingService _loggingService;

        public TestController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpGet("log")]
        public IActionResult LogTest()
        {
            LogMessages();
            return Ok(new { Message = "Logging test completed!" });
        }

        // PRIVATE METHODS
        private void LogMessages()
        {
            _loggingService.LogInformation("This is an information log.");
            _loggingService.LogWarning("This is a warning log.");
            _loggingService.LogError("This is an error log.");
            _loggingService.LogDebug("This is a debug log.");
        }
    }
}
