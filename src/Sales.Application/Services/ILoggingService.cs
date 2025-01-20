namespace Sales.Application.Services
{
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception ex = null);
        void LogDebug(string message);
    }
}
