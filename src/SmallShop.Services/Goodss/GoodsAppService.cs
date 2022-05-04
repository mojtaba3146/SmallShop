using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Goodss
{
    public class GoodsAppService : GoodsService
    {
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;

        public GoodsAppService(GoodsRepository repository
            ,UnitOfWork unitOfWork
            ,CategoryRepository categoryRepository)
        {
            _repository = repository;
            _unitOfWork= unitOfWork;
            _categoryRepository= categoryRepository;
        }

        public void Add(AddGoodsDto dto)
        {
            var isTitleDuplicate = _repository.IsExistGoodsName(dto.Name
                , dto.CategoryId);

            if (isTitleDuplicate)
            {
                throw new GoodsNameIsDuplicatedException();
            }

            var isCategoryExist = _categoryRepository
                .IsCategoryExistById(dto.CategoryId);

            if (!isCategoryExist)
            {
                throw new CategoryNotFoundException();
            }

            var goods = new Goods
            {
                GoodsCode = dto.GoodsCode,
                Name = dto.Name,
                Price = dto.Price,
                MinInventory = dto.MinInventory,
                MaxInventory = dto.MaxInventory,
                CategoryId = dto.CategoryId,
            };

            _repository.Add(goods);
            _unitOfWork.Commit();
        }
    }
}
