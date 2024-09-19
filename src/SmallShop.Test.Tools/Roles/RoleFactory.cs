using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Roles;
using SmallShop.Services.Roles;

namespace SmallShop.Test.Tools.Roles
{
    public static class RoleFactory
    {
        public static RoleAppService CreateService(EFDataContext context)
        {
            var unitOfWork = new EFUnitOfWork(context);
            var repository = new EFRolesRepository(context);

            return new RoleAppService(repository, unitOfWork);
        }
    }
}
