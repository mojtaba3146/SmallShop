using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Services.Users.Contracts;

namespace SmallShop.Persistence.EF.Users
{
    public class EFUserRepository : UserRepository
    {
        private readonly EFDataContext _dbContext;

        public EFUserRepository(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User user)
        {
           _dbContext.Users.Add(user);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users
            .Include(u => u.RefreshTokens)
            .Include(_ => _.Role)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _dbContext.Users
            .Include(u => u.Role)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> IsUserNameExist(string userName)
        {
            return await _dbContext.Users
                .AnyAsync(_ => _.UserName == userName);
        }
    }
}
