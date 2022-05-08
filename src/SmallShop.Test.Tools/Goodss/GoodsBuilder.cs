using SmallShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Test.Tools.Goodss
{
    public class GoodsBuilder
    {
        int _goodsCode;
        Goods goods;
        public GoodsBuilder(int goodsCode)
        {
            _goodsCode = goodsCode;
            goods = new Goods
            {
                GoodsCode = 10,
                Name = "ماست رامک",
                Price = 500,
                MinInventory = 20,
                MaxInventory = 40,
                CategoryId =goodsCode,
                GoodsInventory = 0,
                SellCount= 0
            };
        }

        public Goods Build()
        {
            return goods;
        }

        public GoodsBuilder WithGoodsInventory(int inventory)
        {
            goods.GoodsInventory = inventory;
            return this;
        }

        public GoodsBuilder WithGoodsCode(int goodsCode)
        {
            goods.GoodsCode = goodsCode;
            return this;
        }

        public GoodsBuilder WithName(string name)
        {
            goods.Name = name;
            return this;
        }

        public GoodsBuilder WithSellCount(int sellCount)
        {
            goods.SellCount = sellCount;
            return this;
        }
    }
}
