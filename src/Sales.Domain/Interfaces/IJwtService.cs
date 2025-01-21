namespace Sales.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username);
        bool ValidateToken(string token);
        string GenerateSecureKey(int length = 32);
    }
}
