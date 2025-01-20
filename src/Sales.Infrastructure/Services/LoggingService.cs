using Sales.Application.Services;
using Serilog;

namespace Sales.Infrastructure.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger _logger;

        public LoggingService()
        {
            _logger = Log.Logger;
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            _logger.Error(ex, message);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }
    }
}
