using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.Categories.Contracts;

namespace SmallShop.RestAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _service;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(CategoryService service,
            ILogger<CategoriesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task Add(AddCategoryDto dto)
        {
            await _service.Add(dto);
        }

        [HttpPut]
        public async Task Update(int id,UpdateCategoryDto dto)
        {
            await _service.Update(id,dto);
        }

        [HttpGet]
        public async Task<List<GetAllCategoryDto>> GetAll()
        {
            _logger.LogWarning("This Get All Categories Called at {DT}",
                DateTime.UtcNow.ToLongTimeString());
            return await _service.GetAll();
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
           await _service.Delete(id);
        }

    }
}
