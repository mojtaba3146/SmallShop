using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Goodss.Contracts
{
    public interface GoodsService : Service
    {
        void Add(AddGoodsDto dto);
        List<GetAllGoodsDto> GetAll();
        void Update(int goodsCode, UpdateGoodsDto dto);
        void Delete(int goodsCode);
        GetmaxSellerGoodsDto GetBestSellerGoods();
        List<GetAllGoodsWithMinInvenDto> GetAllMinInventory();
        List<GetAllGoodsWithMaxInvenDto> GetAllMaxInventory();
        List<GetmaxSellerGoodsDto> GetBestSellerGoodsInEchCategory();
        void AddFirstSeed(AddGoodsDto dto);
    }
}
