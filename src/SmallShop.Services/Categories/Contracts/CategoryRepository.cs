using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Categories.Contracts
{
    public interface CategoryRepository : Repository
    {
        void Add(Category category);
        bool ISExistTitle(string title);
        Category GetById(int id);
        List<GetAllCategoryDto> GetAll();
        void Delete(Category category);
        bool IsGoodsExist(int id);
    }
}
