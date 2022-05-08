using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
using SmallShop.Test.Tools.Goodss;
using System;
using System.Linq;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Categories
{
    [Scenario("حذف دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " دسته بندی ها را مدیریت  کنم  ",
        InOrderTo = "هر کالا را در دسته خود قرار دهم"
    )]
    public class DeleteCategoryWhenGoodsExistInThatCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private Category _category;
        private Goods _goods;
        Action expected;
        public DeleteCategoryWhenGoodsExistInThatCategory(ConfigurationFixture configuration) : base(configuration)
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

        [And("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته بندی لبنیات وجود دارد")]
        public void GivenAnd()
        {
            _goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_=>_.Goodss.Add(_goods));
        }

        [When("درخواست حذف دسته بندی با عنوان 'لبنیات' را دارم")]
        public void When()
        {
           expected = () => _sut.Delete(_category.Id);
        }

        [Then("دسته بندی که در آن کالا وجود داشته باشد قابل حذف کردن نیست")]
        public void Then()
        {
            _dataContext.Goodss.Where(_ => _.CategoryId == _category.Id)
                .Should().NotHaveCount(0);
        }

        [And("خطایی با عنوان 'در دسته بندی کالا وجود دارد' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<GoodsExistInCategoryException>();
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
