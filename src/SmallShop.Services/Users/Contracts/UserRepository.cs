using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Users.Contracts
{
    public interface UserRepository : Repository
    {
        void Add(User user);
        Task<bool> IsUserNameExist(string userName);
        Task<User?> GetByUserNameAsync(string username);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
    }
}
