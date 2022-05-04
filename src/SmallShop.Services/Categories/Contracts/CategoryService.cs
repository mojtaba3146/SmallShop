using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Categories.Contracts
{
    public interface CategoryService : Service
    {
        void Add(AddCategoryDto dto);
        void Update(int id,UpdateCategoryDto dto);
        List<GetAllCategoryDto> GetAll();
        void Delete(int id);
    }
}
