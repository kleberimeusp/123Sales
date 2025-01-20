using Moq;
using Sales.Application.Services;

namespace Sales.Tests.Mocks
{
    public static class MockLoggingService
    {
        public static Mock<ILoggingService> GetLoggingService()
        {
            var mockService = new Mock<ILoggingService>();

            mockService.Setup(service => service.LogInformation(It.IsAny<string>())).Verifiable();
            mockService.Setup(service => service.LogWarning(It.IsAny<string>())).Verifiable();
            mockService.Setup(service => service.LogError(It.IsAny<string>(), It.IsAny<System.Exception>())).Verifiable();

            return mockService;
        }
    }
}
