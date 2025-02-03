using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Services.Categories;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Categories
{
    [Scenario("مشاهده دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی ها را مدیریت  کنم  ",
        InOrderTo = "هر کالا را در دسته خود قرار دهم"
    )]
    public class GetAllCategories : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private GetAllCategoryDto _dto;
        public GetAllCategories(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("درخواست مشاهده دسته بندی کالا را می دهم")]
        public async Task When()
        {
            await _sut.GetAll();
        }

        [Then("دسته بندی با عنوان 'لبنیات' نمایش داده می شود")]
        public async Task Then()
        {
            var expected = await _dataContext.Categories.ToListAsync();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_=>_.Title==_category.Title);
        }

        [Fact]
        public async void Run()
        {
            Given();
            await When();
            await Then();
        }
    }
}
