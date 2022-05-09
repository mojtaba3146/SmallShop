using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
