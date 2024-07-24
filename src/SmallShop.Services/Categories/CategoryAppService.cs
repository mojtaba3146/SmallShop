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

        public void Add(AddCategoryDto dto)
        {
            var category = new Category();
            category.Title = dto.Title;

            var isTitleExist=_repository.ISExistTitle(dto.Title);

            if (isTitleExist)
            {
                throw new TitleAlreadyExistException();
            }

            _repository.Add(category);
            _unitOfWork.Commit();
        }

        public int AddSeed(AddCategoryDto dto)
        {
            var isTitleExist = _repository.ISExistTitle(dto.Title);

            if (!isTitleExist)
            {
                var category = new Category();
                category.Title = dto.Title;

                _repository.Add(category);
                _unitOfWork.Commit();

                return category.Id;
            }

            var categoryId = _repository.GetIdByTitle(dto.Title);
            return categoryId;
        }

        public void Delete(int id)
        {
            var category= _repository.GetById(id);

            if (category==null)
            {
                throw new CategoryWithGivenIdDoesNotExist();
            }

            var isGoodsExist = _repository.IsGoodsExist(category.Id);

            if (isGoodsExist)
            {
                throw new GoodsExistInCategoryException();
            }

            _repository.Delete(category);
            _unitOfWork.Commit();
        }

        public List<GetAllCategoryDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id,UpdateCategoryDto dto)
        {
            var category= _repository.GetById(id);

            if (category==null)
            {
                throw new CategoryWithGivenIdDoesNotExist();
            }

            var isTitleExist = _repository.ISExistTitle(dto.Title);

            if (isTitleExist)
            {
                throw new TitleAlreadyExistException();
            }

            category.Title = dto.Title;

            _unitOfWork.Commit();
        }
    }
}
