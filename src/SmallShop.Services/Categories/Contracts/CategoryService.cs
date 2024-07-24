using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Categories.Contracts
{
    public interface CategoryService : Service
    {
        void Add(AddCategoryDto dto);
        void Update(int id,UpdateCategoryDto dto);
        List<GetAllCategoryDto> GetAll();
        void Delete(int id);
        int AddSeed(AddCategoryDto dto);
    }
}
