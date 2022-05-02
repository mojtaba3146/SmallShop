using SmallShop.Entities;
using SmallShop.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Category GetById(int id)
        {
            return _dbContext.Categories.
                FirstOrDefault(c => c.Id == id);
        }

        public bool ISExistTitle(string title)
        {
            return _dbContext.Categories
                .Any(c => c.Title == title);
        }
    }
}
