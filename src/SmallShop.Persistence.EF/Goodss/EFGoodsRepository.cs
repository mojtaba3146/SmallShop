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

        public bool IsExistGoodsName(string name, int categoryId)
        {
            return _dbContext.Goodss.Any(g=>g.Name == name 
            && g.CategoryId == categoryId);
        }
    }
}
