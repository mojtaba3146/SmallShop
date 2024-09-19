using SmallShop.Entities;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Services.Categories;
using SmallShop.Services.Categories.Contracts;

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

        public static CategoryAppService CreateService(EFDataContext context)
        {
            var unitOfWork = new EFUnitOfWork(context);
            var repository = new EFCategoryRepository(context);

            return new CategoryAppService(repository, unitOfWork);
        }
    }
}
