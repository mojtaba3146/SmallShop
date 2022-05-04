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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AddGoodsWithDuplicateTitleInCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private AddGoodsDto _dto;
        Action expected;
        public AddGoodsWithDuplicateTitleInCategory(ConfigurationFixture configuration) : base(configuration)
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
            var goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
        }

        [When("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' تعریف میکنم")]
        public void When()
        {
            _dto = GoodsFactory.CreateAddGoodsDto(_category.Id);

            expected = () => _sut.Add(_dto);
        }

        [Then("تنها یک کالا با عنوان 'ماست رامک' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Goodss.Where(_=>_.Name
            == _dto.Name && _.CategoryId==_category.Id)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان 'عنوان کالا تکراری است' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<GoodsNameIsDuplicatedException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
            ThenAnd();
        }
        
    }
}
