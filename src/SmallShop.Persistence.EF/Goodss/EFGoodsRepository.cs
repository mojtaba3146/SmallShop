using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
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

        public async Task<List<GetAllGoodsDto>> GetAll()
        {
            return await _dbContext.Goodss.Select(x => new GetAllGoodsDto
            {
                GoodsCode = x.GoodsCode,
                Name = x.Name,
                Price = x.Price,
                MinInventory = x.MinInventory,
                MaxInventory = x.MaxInventory,
                CategoryId = x.CategoryId,
            }).ToListAsync();
        }

        public async Task<List<GetAllGoodsWithMaxInvenDto>> GetAllMaxInventory()
        {
            return await _dbContext.Goodss
                .Where(x => x.GoodsInventory >= x.MaxInventory)
                 .Select(x => new GetAllGoodsWithMaxInvenDto
                 {
                     CategoryId = x.CategoryId,
                     Name = x.Name,
                     GoodsCode = x.GoodsCode,
                 }).ToListAsync();
        }

        public async Task<List<GetAllGoodsWithMinInvenDto>> GetAllMinInventory()
        {
            return await _dbContext.Goodss
                .Where(x => x.GoodsInventory <= x.MinInventory)
                .Select(x => new GetAllGoodsWithMinInvenDto
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    GoodsCode = x.GoodsCode,
                }).ToListAsync();
        }

        public async Task<List<GetmaxSellerGoodsDto?>> GetBestSellerGoodsInEchCategory()
        {
            return await _dbContext.Goodss
              .GroupBy(g => g.CategoryId)
              .Select(g => g.OrderByDescending(good => good.SellCount)
                            .Select(good => new GetmaxSellerGoodsDto
                            {
                                Name = good.Name,
                                GoodsCode = good.GoodsCode,
                                CategoryId = good.CategoryId,
                            })
                            .FirstOrDefault())
              .ToListAsync();
        }

        public async Task<GetmaxSellerGoodsDto?> GetBestSellerGoods()
        {
            return await _dbContext.Goodss.
               OrderByDescending(x => x.SellCount)
               .Select(x => new GetmaxSellerGoodsDto
               {
                   Name = x.Name,
                   GoodsCode = x.GoodsCode,
                   CategoryId = x.CategoryId,
               }).FirstOrDefaultAsync();
        }

        public async Task<Goods?> GetById(int goodsCode)
        {
            return await _dbContext.Goodss.
                FirstOrDefaultAsync(_ => _.GoodsCode == goodsCode);
        }

        public async Task<bool> IsExistGoodsName(string name, int categoryId)
        {
            return await _dbContext.Goodss.AnyAsync(g=>g.Name == name 
            && g.CategoryId == categoryId);
        }

        public async Task<bool> IsExistGoodsNameDuplicate(string name, int categoryId, int goodsCode)
        {
            return await _dbContext.Goodss.AnyAsync(g => g.Name == name
            && g.CategoryId == categoryId && g.GoodsCode != goodsCode);
        }
    }
}
