using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Services.Roles.Contracts;

namespace SmallShop.Persistence.EF.Roles
{
    public class EFRolesRepository : RoleRepository
    {
        private readonly EFDataContext _dbContext;

        public EFRolesRepository(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Role role)
        {
            _dbContext.Roles.Add(role);
        }

        public int GetRoleIdByName(string roleName)
        {
            return  _dbContext.Roles.First(_ => _.Name == roleName).Id;
        }

        public async Task<bool> IsRoleExist(string roleName)
        {
            return await _dbContext.Roles
                .AnyAsync(_ => _.Name.ToLower() == roleName.ToLower());
        }
    }
}
