using SmallShop.Infrastructure.Application;
using SmallShop.Services.Users.Contracts.Dtos;

namespace SmallShop.Services.Users.Contracts
{
    public interface UserService : Service
    {
        Task<int> Add(AddUserDto dto);
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto,string ipAddress);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress);
    }
}
