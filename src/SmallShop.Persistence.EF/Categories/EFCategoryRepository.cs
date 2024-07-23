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

        public List<GetAllCategoryDto> GetAll()
        {
            return _dbContext.Categories.Select(c => new GetAllCategoryDto
            {
                Title = c.Title,
            }).ToList();
        }

        public Category? GetById(int id)
        {
            return _dbContext.Categories.
                FirstOrDefault(c => c.Id == id);
        }

        public bool IsCategoryExistById(int categoryId)
        {
            return _dbContext.Categories.Any(
                _=>_.Id == categoryId);
        }

        public bool ISExistTitle(string title)
        {
            return _dbContext.Categories
                .Any(c => c.Title == title);
        }

        public bool IsGoodsExist(int id)
        {
            return _dbContext.Goodss.
                Any(_ => _.CategoryId==id);

        }
    }
}
