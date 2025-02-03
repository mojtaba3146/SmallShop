using SmallShop.Entities;
using SmallShop.Infrastructure.Application;

namespace SmallShop.Services.Goodss.Contracts
{
    public interface GoodsRepository : Repository
    {
        void Add(Goods goods);
        Task<bool> IsExistGoodsName(string name, int categoryId);
        Task<List<GetAllGoodsDto>> GetAll();
        Task<Goods?> GetById(int goodsCode);
        void Delete(Goods goods);
        Task<bool> IsExistGoodsNameDuplicate(string name, int categoryId, int goodsCode);
        Task<GetmaxSellerGoodsDto?> GetBestSellerGoods();
        Task<List<GetAllGoodsWithMinInvenDto>> GetAllMinInventory();
        Task<List<GetAllGoodsWithMaxInvenDto>> GetAllMaxInventory();
        Task<List<GetmaxSellerGoodsDto?>> GetBestSellerGoodsInEchCategory();
    }
}
