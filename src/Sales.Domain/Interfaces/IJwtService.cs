using System.Security.Claims;

namespace Sales.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username);
        ClaimsPrincipal ValidateToken(string token);
    }
}
