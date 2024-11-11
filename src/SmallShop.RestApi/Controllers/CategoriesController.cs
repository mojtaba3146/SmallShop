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
        public List<GetAllCategoryDto> GetAll()
        {
            _logger.LogWarning("This Get All Categories Called at {DT}",
                DateTime.UtcNow.ToLongTimeString());
            return _service.GetAll();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

    }
}
