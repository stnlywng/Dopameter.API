namespace Dopameter.Services;

public interface IAuthService
{
    public string GenerateJwtToken(string userId, string username);
}