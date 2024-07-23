using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool ISExistTitle(string title);
        Category? GetById(int id);
        List<GetAllCategoryDto> GetAll();
        void Delete(Category category);
        bool IsGoodsExist(int id);
        bool IsCategoryExistById(int categoryId);
    }
}
