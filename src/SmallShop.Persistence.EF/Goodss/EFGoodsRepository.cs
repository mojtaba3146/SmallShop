using SmallShop.Entities;
using SmallShop.Services.Goodss.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
