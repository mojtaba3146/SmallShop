using FluentAssertions;
using SmallShop.Infrastructure.Application;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Services.Categories;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using System.Linq;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی ها را مدیریت  کنم  ",
        InOrderTo = "هر کالا را در دسته خود قرار دهم"
    )]
    public class AddCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private AddCategoryDto _dto;

        public AddCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository,_unitOfWork);
        }

        [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد")]
        public void Given()
        {

        }

        [When("دسته بندی با عنوان 'لبنیات' تعریف می کنم")]
        public void When()
        {
            _dto=CategoryFactory.CreateAddCategoryDto("لبنیات");

            _sut.Add(_dto);
        }

        [Then("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا باید وجود داشته باشد")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();

            expected.Title.Should().Be(_dto.Title);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }   
    }
}
