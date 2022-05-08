using FluentAssertions;
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
using System.Linq;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Goodss
{
    [Scenario("حذف کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را حذف  کنم  ",
        InOrderTo = "کالا را از لیست کالا ها حذف کنم"
    )]
    public class DeleteGoods : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Goods _goods;

        public DeleteGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }

        [When("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' را حذف میکنم")]
        public void When()
        {
            _sut.Delete(_goods.GoodsCode);
        }

        [Then("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' نباید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Goodss.ToList();

            expected.Should().HaveCount(0);
            expected.Should().NotContain(_ => _.Name == _goods.Name);
            expected.Should().NotContain(_ => _.Price == _goods.Price);
            expected.Should().NotContain(_ => _.MinInventory == _goods.MinInventory);
            expected.Should().NotContain(_ => _.MaxInventory == _goods.MaxInventory);
            expected.Should().NotContain(_ => _.CategoryId == _goods.CategoryId);
            expected.Should().NotContain(_ => _.GoodsCode == _goods.GoodsCode);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
        }
    }
}
