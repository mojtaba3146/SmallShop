using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
