using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Categories.Exceptions;

namespace SmallShop.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CategoryAppService(
            CategoryRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(AddCategoryDto dto)
        {
            var isTitleExist = await _repository.IsExistTitle(dto.Title);

            if (isTitleExist)
            {
                throw new TitleAlreadyExistException();
            }
            var category = new Category {Title = dto.Title };
           
            _repository.Add(category);
            await _unitOfWork.Commit();
        }

        public async Task<int> AddSeed(AddCategoryDto dto)
        {
            var categoryId = await _repository.GetIdByTitle(dto.Title);

            if (categoryId!.Value is not 0)
            {
                return categoryId.Value;
            }

            var category = new Category
            {
                Title = dto.Title
            };

            _repository.Add(category);
            await _unitOfWork.Commit();

            return category.Id;

        }

        public async Task Delete(int id)
        {
            var category = await _repository.GetById(id);

            if (category == null)
            {
                throw new CategoryWithGivenIdDoesNotExist();
            }

            var isGoodsExist = await _repository.IsGoodsExist(category.Id);

            if (isGoodsExist)
            {
                throw new GoodsExistInCategoryException();
            }

            _repository.Delete(category);
            await _unitOfWork.Commit();
        }

        public async Task<List<GetAllCategoryDto>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task Update(int id, UpdateCategoryDto dto)
        {
            var category = await _repository.GetById(id);

            if (category == null)
            {
                throw new CategoryWithGivenIdDoesNotExist();
            }

            var isTitleExist = await _repository.IsExistTitle(dto.Title);

            if (isTitleExist)
            {
                throw new TitleAlreadyExistException();
            }

            category.Title = dto.Title;

           await _unitOfWork.Commit();
        }
    }
}
