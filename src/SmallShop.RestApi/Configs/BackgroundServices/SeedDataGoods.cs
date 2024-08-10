using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss.Contracts;

namespace SmallShop.RestApi.Configs.BackgroundServices
{
    public class SeedDataGoods : IHostedService

    {
        private readonly CategoryService _categoryService;
        private readonly GoodsService _goodsService;
        private readonly IConfiguration _configuration;

        public SeedDataGoods(CategoryService categoryService,
            GoodsService goodsService,
            IConfiguration configuration)
        {
            _categoryService = categoryService;
            _goodsService = goodsService;
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {

            var categoryId = _categoryService.AddSeed(new AddCategoryDto
            {
                Title = _configuration.GetSection("Category:Title").Value!
            });

            _goodsService.AddFirstSeed(new AddGoodsDto
            {
                CategoryId = categoryId,
                GoodsCode = 1001,
                Name = "MojiBook",
                Price = 10000,
                MinInventory = 0,
                MaxInventory = 100
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
