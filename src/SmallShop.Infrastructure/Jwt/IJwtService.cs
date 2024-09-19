namespace SmallShop.Infrastructure.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(string userName, string role);
        string GenerateRefreshToken();
    }
}
