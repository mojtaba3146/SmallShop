using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Services.Categories;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Categories.Exceptions;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Categories
{
    [Scenario("ویرایش دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی ها را مدیریت  کنم  ",
        InOrderTo = "هر کالا را در دسته خود قرار دهم"
    )]
    public class UpdateCategoryWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private UpdateCategoryDto _dto;
        Func<Task> expected;
        public UpdateCategoryWithDuplicateTitle(ConfigurationFixture configuration) : base(configuration)
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
        [And("دسته بندی با عنوان 'خشکبار' در فهرست دسته بندی کالا وجود دارد")]
        public void GivenAnd()
        {
            var category = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        [When("دسته بندی با عنوان  ‘لبنیات ’را به ‘خشکبار’  تغییر می دهم")]
        public async Task When()
        {
            var category = _dataContext.Categories.Where(c => c.Title == "لبنیات");
            _dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            expected = async () => await _sut.Update(_category.Id, _dto);
        }

        [Then("تنها یک دسته بندی با عنوان 'خشکبار' باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Where(c => c.Title ==
           _dto.Title).Should().HaveCount(1);
        }

        [And("خطایی با عنوان ‘عنوان دسته بندی کالا تکراری است’ باید رخ دهد")]
        public async Task ThenAnd()
        {
            await expected.Should().ThrowExactlyAsync<TitleAlreadyExistException>();
        }

        [Fact]
        public async void Run()
        {
            Given();
            GivenAnd();
            await When();
            Then();
            await ThenAnd();
        }

    }
}
