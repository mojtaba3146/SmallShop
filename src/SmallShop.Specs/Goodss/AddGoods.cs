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
    [Scenario("تعریف کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را تعریف  کنم  ",
        InOrderTo = "کالایی به لیست کالاها اضافه کنم"
    )]
    public class AddGoods : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private AddGoodsDto _dto;
        public AddGoods(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository= new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork,_categoryRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود دارد ")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("هیچ کالایی در فهرست کالا وجود ندارد")]
        public void And()
        {

        }

        [When(" کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' تعریف میکنم ")]
        public async Task When()
        {
            _dto = GoodsFactory.CreateAddGoodsDto(_category.Id);

            await _sut.Add(_dto);
        }

        [Then("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' باید وجود داشته باشد")]
        public async Task Then()
        {
            var expected = await _dataContext.Goodss.FirstOrDefaultAsync();

            expected!.Name.Should().Be(_dto.Name);
            expected.GoodsCode.Should().Be(_dto.GoodsCode);
            expected.MinInventory.Should().Be(_dto.MinInventory);
            expected.MaxInventory.Should().Be(_dto.MaxInventory);
            expected.Price.Should().Be(_dto.Price);
            expected.CategoryId.Should().Be(_category.Id);
        }

        [Fact]
        public async void Run()
        {
            Given();
            And();
            await When();
            await Then();
        }
    }
}
