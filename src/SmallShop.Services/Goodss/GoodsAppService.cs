using SmallShop.Entities;
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

        public async Task Add(AddGoodsDto dto)
        {
            var isTitleDuplicate = await _repository.IsExistGoodsName(dto.Name
                , dto.CategoryId);

            if (isTitleDuplicate)
            {
                throw new GoodsNameIsDuplicatedException();
            }

            var isCategoryExist = await _categoryRepository
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
            await _unitOfWork.Commit();
        }

        public async Task AddFirstSeed(AddGoodsDto dto)
        {
            var isSeedDataExist = await _repository.IsExistGoodsName(dto.Name
               , dto.CategoryId);

            if (!isSeedDataExist)
            {
                var isCategoryExist = await _categoryRepository
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
                await _unitOfWork.Commit();
            }

        }

        public async Task Delete(int goodsCode)
        {
            var goods = await _repository.GetById(goodsCode);

            if (goods == null)
            {
                throw new GoodsDoesNotExistException();
            }

            _repository.Delete(goods);

            await _unitOfWork.Commit();
        }

        public async Task<List<GetAllGoodsDto>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<List<GetAllGoodsWithMaxInvenDto>> GetAllMaxInventory()
        {
            return await _repository.GetAllMaxInventory();
        }

        public async Task<List<GetAllGoodsWithMinInvenDto>> GetAllMinInventory()
        {
            return await _repository.GetAllMinInventory();
        }

        public async Task<GetmaxSellerGoodsDto?> GetBestSellerGoods()
        {
            return await _repository.GetBestSellerGoods();
        }

        public async Task<List<GetmaxSellerGoodsDto>> GetBestSellerGoodsInEchCategory()
        {
            return await _repository.GetBestSellerGoodsInEchCategory();
        }

        public async Task Update(int goodsCode, UpdateGoodsDto dto)
        {
            var goods = await _repository.GetById(goodsCode);

            if (goods == null)
            {
                throw new GoodsDoesNotExistException();
            }

            var isTitleDuplicate = await _repository.IsExistGoodsNameDuplicate(dto.Name
                , dto.CategoryId,goodsCode);

            if (isTitleDuplicate)
            {
                throw new GoodsNameIsDuplicatedException();
            }

            var isCategoryExist = await _categoryRepository
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

            await _unitOfWork.Commit();
        }
    }
}
