using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Goodss.Contracts
{
    public interface GoodsService : Service
    {
        Task Add(AddGoodsDto dto);
        Task<List<GetAllGoodsDto>> GetAll();
        Task Update(int goodsCode, UpdateGoodsDto dto);
        Task Delete(int goodsCode);
        Task<GetmaxSellerGoodsDto?> GetBestSellerGoods();
        Task<List<GetAllGoodsWithMinInvenDto>> GetAllMinInventory();
        Task<List<GetAllGoodsWithMaxInvenDto>> GetAllMaxInventory();
        Task<List<GetmaxSellerGoodsDto>> GetBestSellerGoodsInEchCategory();
        Task AddFirstSeed(AddGoodsDto dto);
    }
}
