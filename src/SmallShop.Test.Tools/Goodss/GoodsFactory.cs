using SmallShop.Entities;
using SmallShop.Services.Goodss.Contracts;

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
                GoodsInventory=5
            };
        }

        public static UpdateGoodsDto CreateUpdateGoodsDto(int categoryId)
        {
            return new UpdateGoodsDto
            {
                GoodsCode = 10,
                Name = "ماست رامک",
                Price = 700,
                MinInventory = 20,
                MaxInventory = 40,
                CategoryId = categoryId,
            };
        }

        public static AddGoodsDto CreateAddGoodsDto(int categoryId)
        {
            return new AddGoodsDto
            {
                GoodsCode = 10,
                Name = "ماست رامک",
                Price = 500,
                MinInventory = 20,
                MaxInventory = 40,
                CategoryId = categoryId,
            };
        }

        public static Goods CreateGoods(int categoryId)
        {
            return new Goods
            {
                GoodsCode = 11,
                Name = "ماست میهن",
                Price = 500,
                MinInventory = 20,
                MaxInventory = 40,
                CategoryId = categoryId,
            };
        }

        public static UpdateGoodsDto CreateUpdateGoods(int categoryId)
        {
            return new UpdateGoodsDto
            {
                GoodsCode = 10,
                Name = "ماست میهن",
                Price = 500,
                MinInventory = 20,
                MaxInventory = 40,
                CategoryId = categoryId,
            };
        }
    }
}
