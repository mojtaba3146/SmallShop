using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.Goodss.Contracts;

namespace SmallShop.RestApi.Controllers
{
    [Route("api/Goodss")]
    [ApiController]
    public class GoodssController : ControllerBase
    {
        private readonly GoodsService _service;

        public GoodssController(GoodsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task Add(AddGoodsDto dto)
        {
            await _service.Add(dto);
        }

        [HttpGet]
        public async Task<List<GetAllGoodsDto>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpPut]
        public async Task Update(int goodsCode,UpdateGoodsDto dto)
        {
            await _service.Update(goodsCode,dto);
        }

        [HttpDelete]
        public async Task Delete(int goodsCode)
        {
            await _service.Delete(goodsCode);
        }

        [HttpGet("/maxsell")]
        public async Task<GetmaxSellerGoodsDto?> GetMaxSellGoods()
        {
           return await _service.GetBestSellerGoods();
        }

        [HttpGet("/mininventory")]
        public async Task<List<GetAllGoodsWithMinInvenDto>> GetMinInventory()
        {
            return await _service.GetAllMinInventory();
        }

        [HttpGet("/maxinventory")]
        public async Task<List<GetAllGoodsWithMaxInvenDto>> GetMaxInventory()
        {
            return await _service.GetAllMaxInventory();
        }

        [HttpGet("/maxsellincategory")]
        public async Task<List<GetmaxSellerGoodsDto>> GetBestSellerInEachCategory()
        {
            return await _service.GetBestSellerGoodsInEchCategory();
        }
    }
}
