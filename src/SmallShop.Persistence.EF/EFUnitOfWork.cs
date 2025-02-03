using SmallShop.Infrastructure.Application;

namespace SmallShop.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _dbContext;

        public EFUnitOfWork(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
