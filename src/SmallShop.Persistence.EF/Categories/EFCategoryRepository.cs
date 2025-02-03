using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Services.Categories.Contracts;


namespace SmallShop.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dbContext;

        public EFCategoryRepository(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Category category)
        {
            _dbContext.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _dbContext.Categories.Remove(category);
        }

        public async Task<List<GetAllCategoryDto>> GetAll()
        {
            return await _dbContext.Categories.Select(c => new GetAllCategoryDto
            {
                Title = c.Title,
            }).ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int?> GetIdByTitle(string title)
        {
            return await _dbContext.Categories
                .Where(x => x.Title == title)
                .Select(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsCategoryExistById(int categoryId)
        {
            return await _dbContext.Categories.AnyAsync(
                _=>_.Id == categoryId);
        }

        public async Task<bool> IsExistTitle(string title)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.Title == title);
        }

        public async Task<bool> IsGoodsExist(int id)
        {
            return await _dbContext.Goodss.
                AnyAsync(_ => _.CategoryId==id);

        }
    }
}
