using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Roles.Contracts;
using SmallShop.Services.Roles.Contracts.Dtos;
using SmallShop.Services.Roles.Exceptions;

namespace SmallShop.Services.Roles
{
    public class RoleAppService : RoleService
    {
        private readonly RoleRepository _roleRepository;
        private readonly UnitOfWork _unitOfWork;

        public RoleAppService(RoleRepository roleRepository,
            UnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AddRoleDto Dto)
        {
            var isRoleExist = await _roleRepository.IsRoleExist(Dto.RoleName);
            if (isRoleExist)
            {
                throw new RoleIsDuplicatedException();
            }

            var role = new Role { Name = Dto.RoleName };

            _roleRepository.Add(role);
            await _unitOfWork.Commit();    
        }
    }
}
