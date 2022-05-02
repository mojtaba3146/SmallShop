using SmallShop.Entities;
using SmallShop.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Test.Tools.Categories
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string title)
        {
            return  new Category
            {
                Title = title,
            };   
        }

        public static UpdateCategoryDto CreateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDto
            {
                Title = "خشکبار"
            };
        }

        public static AddCategoryDto CreateAddCategoryDto(string title)
        {
            return new AddCategoryDto
            {
                Title = title
            };
        }
    }
}
