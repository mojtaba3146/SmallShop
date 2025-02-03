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
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Goodss
{
    [Scenario("ویرایش کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را ویرایش  کنم  ",
        InOrderTo = "لیست کالا تصحیح شود"
    )]
    public class UpdateGoodsWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private UpdateGoodsDto _dto;
        private Category _category;
        private Goods _goods;
        private Func<Task> expected;

        public UpdateGoodsWithDuplicateName(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود دارد ")]
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

        [And("کالایی به نام 'ماست میهن' و قیمت '500' و کد کالای '11' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' وجود دارد")]
        public void GivenAndTwo()
        {
            var goods = GoodsFactory.CreateGoods(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
        }

        [When("کالایی به نام 'ماست میهن' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' را ویرایش میکنم")]
        public async Task When()
        {
            _dto = GoodsFactory.CreateUpdateGoods(_category.Id);

            expected = async () => await _sut.Update(_goods.GoodsCode, _dto);
        }

        [Then("تنها یک کالا با عنوان 'ماست میهن' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Goodss.Where(_ => _.Name
            == _dto.Name && _.CategoryId == _category.Id)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'عنوان کالا تکراری است' باید رخ دهد")]
        public async Task ThenAnd()
        {
            await expected.Should().ThrowExactlyAsync<GoodsNameIsDuplicatedException>();
        }

        [Fact]
        public async void Run()
        {
            Given();
            GivenAnd();
            GivenAndTwo();
            await When();
            Then();
            await ThenAnd();
        }
    }
}
