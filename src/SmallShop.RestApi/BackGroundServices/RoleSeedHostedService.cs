using SmallShop.Entities;
using SmallShop.Services.Roles.Contracts;
using SmallShop.Services.Roles.Contracts.Dtos;

namespace SmallShop.RestApi.BackGroundServices
{
    public class RoleSeedHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public RoleSeedHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleRepository = scope.ServiceProvider.GetRequiredService<RoleRepository>();
                var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();

                await SeedRoleIfNotExistsAsync(roleRepository, roleService, SystemRole.Manager);
                await SeedRoleIfNotExistsAsync(roleRepository, roleService, SystemRole.Employee);
            }
        }

        private async Task SeedRoleIfNotExistsAsync(RoleRepository roleRepository,
            RoleService roleService, string roleName)
        {
            var roleExists = await roleRepository.IsRoleExist(roleName);
            if (!roleExists)
            {
                var roleDto = new AddRoleDto { RoleName = roleName };
                await roleService.AddAsync(roleDto);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
