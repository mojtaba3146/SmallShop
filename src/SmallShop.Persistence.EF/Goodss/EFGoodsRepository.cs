﻿using SmallShop.Entities;
using SmallShop.Services.Goodss.Contracts;
using System.Collections.Generic;
using System.Linq;

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
           List<int> categoryId = new List<int>();
            List<GetmaxSellerGoodsDto> maxSellgoods = new 
                List<GetmaxSellerGoodsDto>();
            var ids = _dbContext.Categories.Select(x => x.Id);
            categoryId.AddRange(ids);

            foreach (var id in categoryId)
            {
                var goods = _dbContext.Goodss.Where(x => x.CategoryId == id);
                maxSellgoods.Add(goods.OrderByDescending(x => x.SellCount)
                    .Select(x =>
                      new GetmaxSellerGoodsDto
                      {
                          Name = x.Name,
                          GoodsCode = x.GoodsCode,
                          CategoryId = id,
                      }).FirstOrDefault());
            }  
            return maxSellgoods;
        }

        public GetmaxSellerGoodsDto GetBestSellerGoods()
        {
            return (GetmaxSellerGoodsDto)_dbContext.Goodss.
               OrderByDescending(x => x.SellCount)
               .Select(x => new GetmaxSellerGoodsDto
               {
                   Name = x.Name,
                   GoodsCode = x.GoodsCode,
                   CategoryId = x.CategoryId,
               }).FirstOrDefault();
        }

        public Goods GetById(int goodsCode)
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
