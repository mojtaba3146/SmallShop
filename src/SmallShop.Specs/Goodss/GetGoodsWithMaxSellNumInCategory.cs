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
    [Scenario("مشاهده کالای پر فروش در هر دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "  مشاهده کالای پر فروش در هر دسته بندی ",
        InOrderTo = "پرفروش تریت کالا را در هر دسته بندی مشاهده کنم"
    )]
    public class GetGoodsWithMaxSellNumInCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Goods _goods, _goodsTwo;
        public GetGoodsWithMaxSellNumInCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' و تعداد فروش '5' در دسته 'لبنیات' وجود دارد")]
        public void Given()
        {
            CreateFirstGoods();
        }

        [And("کالایی به نام 'نوشابه کولا' و قیمت '500' و کد کالای '12' و حداقل موجودی '20' و حداکثر موجودی '40' و تعداد فروش '15' در دسته 'نوشیدنی' وجود دارد")]
        public void GivenAnd()
        {
            CreateSecondGoods();
        }

        [When("درخواست مشاهده کالایی با فروش بیشتر در هر دسته بندی را دارم")]
        public async Task When()
        {
            await _sut.GetBestSellerGoodsInEchCategory();
        }

        [Then("کالایی به نام 'ماست رامک' در دسته 'لبنیات' و کالایی به نام 'نوشابه کولا' در دسته 'نوشیدنی' نشان داده می شود")]
        public async Task Then()
        {
            var expected =await _dataContext.Goodss.ToListAsync();

            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.Name == _goodsTwo.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goodsTwo.GoodsCode);
        }

        [Fact]
        public async void Run()
        {
            Given();
            GivenAnd();
            await When();
            await Then();
        }

        private void CreateFirstGoods()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id).WithSellCount(5)
                .WithGoodsInventory(21).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }

        private void CreateSecondGoods()
        {
            _category = CategoryFactory.CreateCategory("نوشیدنی");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goodsTwo = new GoodsBuilder(_category.Id).WithSellCount(15)
                .WithGoodsInventory(21).WithGoodsCode(12).
                WithName("نوشابه کولا").Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goodsTwo));
        }
    }
}
