using Microsoft.AspNetCore.Http;
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
        public void Add(AddGoodsDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public void GetAll()
        {
            _service.GetAll();
        }

        [HttpPut]
        public void Update(int goodsCode,UpdateGoodsDto dto)
        {
            _service.Update(goodsCode,dto);
        }

        [HttpDelete]
        public void Delete(int goodsCode)
        {
            _service.Delete(goodsCode);
        }
    }
}
