using SmallShop.Infrastructure.Application;
using SmallShop.Services.Roles.Contracts.Dtos;

namespace SmallShop.Services.Roles.Contracts
{
    public interface RoleService : Service
    {
        Task AddAsync(AddRoleDto roleDto);
    }
}
