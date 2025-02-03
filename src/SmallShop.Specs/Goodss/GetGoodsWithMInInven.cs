using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Goodss
{
    [Scenario("مشاهده کالای با موجودی کمتر از حداقل موجودی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "  مشاهده کالا هایی با موجودی کمتر از حداقل موجودی ",
        InOrderTo = "از آن کالا ها سفارش دهم"
    )]
    public class GetGoodsWithMInInven : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Goods _goods;
        public GetGoodsWithMInInven(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' و موجودی '15' در دسته 'لبنیات' وجود دارد")]
        public void Given()
        {
            CreateGoods();
        }

        [When("درخواست مشاهده کالایی با موجودی کمتر از حداقل موجودی را دارم")]
        public async Task When()
        {
            await _sut.GetAllMinInventory();
        }

        [Then("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' و موجودی '15' در دسته 'لبنیات' نمایش داده می شود")]
        public async Task Then()
        {
            var expected = await _dataContext.Goodss.ToListAsync();

            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
        }

        [Fact]
        public async void Run()
        {
            Given();
            await When();
            await Then();
        }

        private void CreateGoods()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id)
                .WithGoodsInventory(15).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }
    }
}
