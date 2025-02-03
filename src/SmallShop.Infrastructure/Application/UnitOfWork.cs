namespace SmallShop.Infrastructure.Application
{
    public interface UnitOfWork
    {
        Task Commit();
    }
}
