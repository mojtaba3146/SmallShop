using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Roles.Contracts
{
    public interface RoleRepository : Repository
    {
        void Add(Role role);
        Task<bool> IsRoleExist(string roleName);
        int GetRoleIdByName(string roleName);
    }
}
