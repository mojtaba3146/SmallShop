using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Services.Roles.Contracts;
using SmallShop.Services.Roles.Contracts.Dtos;
using SmallShop.Services.Roles.Exceptions;
using SmallShop.Test.Tools.Roles;
using Xunit;

namespace SmallShop.Services.Test.Unit.Roles
{
    public class RoleServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly RoleService _sut;

        public RoleServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _sut = RoleFactory.CreateService(_dataContext);
        }

        [Fact]
        public async Task AddAsync_ShouldAddRole_WhenRoleDoesNotExist()
        {
            var roleName = "NewRole";
            var dto = new AddRoleDto { RoleName = roleName };

            await _sut.AddAsync(dto);

            var role = await _dataContext.Roles.FirstAsync();
            role.Should().NotBeNull();
            role.Name.Should().Be(roleName);
        }

        [Fact]
        public async Task Add_throws_RoleIsDuplicatedException_when_role_name_already_exists()
        {
            var role = new Role { Name = "dummyRole" };
            _dataContext.Manipulate(_ => _.Roles.Add(role));
            var dto = new AddRoleDto { RoleName = role.Name };

            var expected = async () => await _sut.AddAsync(dto);

            await expected.Should()
                .ThrowExactlyAsync<RoleIsDuplicatedException>();
        }
    }
}
