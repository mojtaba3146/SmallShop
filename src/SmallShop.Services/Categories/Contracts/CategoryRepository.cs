using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        Task<bool> IsExistTitle(string title);
        Task<Category?> GetById(int id);
        Task<List<GetAllCategoryDto>> GetAll();
        void Delete(Category category);
        Task<bool> IsGoodsExist(int id);
        Task<bool> IsCategoryExistById(int categoryId);
        Task<int?> GetIdByTitle(string title);
    }
}
