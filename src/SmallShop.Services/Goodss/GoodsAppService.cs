﻿using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;


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

        public void AddFirstSeed(AddGoodsDto dto)
        {
            var isSeedDataExist = _repository.IsExistGoodsName(dto.Name
               , dto.CategoryId);

            if (!isSeedDataExist)
            {
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

        public void Delete(int goodsCode)
        {
            var goods = _repository.GetById(goodsCode);

            if (goods == null)
            {
                throw new GoodsDoesNotExistException();
            }

            _repository.Delete(goods);

            _unitOfWork.Commit();
        }

        public List<GetAllGoodsDto> GetAll()
        {
            return _repository.GetAll();
        }

        public List<GetAllGoodsWithMaxInvenDto> GetAllMaxInventory()
        {
            return _repository.GetAllMaxInventory();
        }

        public List<GetAllGoodsWithMinInvenDto> GetAllMinInventory()
        {
            return _repository.GetAllMinInventory();
        }

        public GetmaxSellerGoodsDto GetBestSellerGoods()
        {
            return _repository.GetBestSellerGoods();
        }

        public List<GetmaxSellerGoodsDto> GetBestSellerGoodsInEchCategory()
        {
            return _repository.GetBestSellerGoodsInEchCategory();
        }

        public void Update(int goodsCode, UpdateGoodsDto dto)
        {
            Goods goods = _repository.GetById(goodsCode);

            if (goods == null)
            {
                throw new GoodsDoesNotExistException();
            }

            var isTitleDuplicate = _repository.IsExistGoodsNameDuplicate(dto.Name
                , dto.CategoryId,goodsCode);

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

            goods.Name = dto.Name;
            goods.Price = dto.Price;
            goods.MinInventory = dto.MinInventory;
            goods.MaxInventory = dto.MaxInventory;
            goods.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }
    }
}
