using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.Categories.Contracts;

namespace SmallShop.RestAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpPut]
        public void Update(int id,UpdateCategoryDto dto)
        {
            _service.Update(id,dto);
        }

        [HttpGet]
        public void GetAll()
        {
            _service.GetAll();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

    }
}
