using SmallShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Test.Tools.Goodss
{
    public static class GoodsFactory
    {
        public static Goods CreateGoodsWithCategory(int categoryId)
        {
            return new Goods
            {
                GoodsCode = 10,
                Name = "ماست رامک",
                Price = 500,
                MinInventory = 20,
                MaxInventory = 40,
                CategoryId = categoryId,
            };
        }
    }
}
