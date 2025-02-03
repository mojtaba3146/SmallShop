using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Categories.Contracts
{
    public interface CategoryService : Service
    {
        Task Add(AddCategoryDto dto);
        Task Update(int id,UpdateCategoryDto dto);
        Task<List<GetAllCategoryDto>> GetAll();
        Task Delete(int id);
        Task<int> AddSeed(AddCategoryDto dto);
    }
}
