﻿using SmallShop.Entities;
using SmallShop.Services.Goodss.Contracts;

namespace SmallShop.Persistence.EF.Goodss
{
    public class EFGoodsRepository : GoodsRepository
    {
        private readonly EFDataContext _dbContext;

        public EFGoodsRepository(EFDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Goods goods)
        {
            _dbContext.Goodss.Add(goods);
        }

        public void Delete(Goods goods)
        {
            _dbContext.Goodss.Remove(goods);
        }

        public List<GetAllGoodsDto> GetAll()
        {
            return _dbContext.Goodss.Select(x => new GetAllGoodsDto
            {
                GoodsCode = x.GoodsCode,
                Name = x.Name,
                Price = x.Price,
                MinInventory = x.MinInventory,
                MaxInventory = x.MaxInventory,
                CategoryId = x.CategoryId,
            }).ToList();
        }

        public List<GetAllGoodsWithMaxInvenDto> GetAllMaxInventory()
        {
            return _dbContext.Goodss
                .Where(x => x.GoodsInventory >= x.MaxInventory)
                 .Select(x => new GetAllGoodsWithMaxInvenDto
                 {
                     CategoryId = x.CategoryId,
                     Name = x.Name,
                     GoodsCode = x.GoodsCode,
                 }).ToList();
        }

        public List<GetAllGoodsWithMinInvenDto> GetAllMinInventory()
        {
            return _dbContext.Goodss
                .Where(x => x.GoodsInventory <= x.MinInventory)
                .Select(x => new GetAllGoodsWithMinInvenDto
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    GoodsCode = x.GoodsCode,
                }).ToList();
        }

        public List<GetmaxSellerGoodsDto> GetBestSellerGoodsInEchCategory()
        {
            var maxSellGoods = new List<GetmaxSellerGoodsDto>();

            var categoryIds = _dbContext.Categories.Select(x => x.Id).ToList();

            foreach (var categoryId in categoryIds)
            {
                var bestSeller = _dbContext.Goodss
                    .Where(x => x.CategoryId == categoryId)
                    .OrderByDescending(x => x.SellCount)
                    .Select(x => new GetmaxSellerGoodsDto
                    {
                        Name = x.Name,
                        GoodsCode = x.GoodsCode,
                        CategoryId = categoryId,
                    })
                    .FirstOrDefault();

                if (bestSeller != null)
                {
                    maxSellGoods.Add(bestSeller);
                }
            }

            return maxSellGoods;
        }

        public GetmaxSellerGoodsDto? GetBestSellerGoods()
        {
            return _dbContext.Goodss.
               OrderByDescending(x => x.SellCount)
               .Select(x => new GetmaxSellerGoodsDto
               {
                   Name = x.Name,
                   GoodsCode = x.GoodsCode,
                   CategoryId = x.CategoryId,
               }).FirstOrDefault();
        }

        public Goods? GetById(int goodsCode)
        {
            return _dbContext.Goodss.
                FirstOrDefault(_ => _.GoodsCode == goodsCode);
        }

        public bool IsExistGoodsName(string name, int categoryId)
        {
            return _dbContext.Goodss.Any(g=>g.Name == name 
            && g.CategoryId == categoryId);
        }

        public bool IsExistGoodsNameDuplicate(string name, int categoryId, int goodsCode)
        {
            return _dbContext.Goodss.Any(g => g.Name == name
            && g.CategoryId == categoryId && g.GoodsCode != goodsCode);
        }
    }
}
