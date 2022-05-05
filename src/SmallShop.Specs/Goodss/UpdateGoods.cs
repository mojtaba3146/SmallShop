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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class UpdateGoods : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private UpdateGoodsDto _dto;
        private Category _category;
        private Goods _goods;
        public UpdateGoods(ConfigurationFixture configuration) : base(configuration)
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

        [When("کالایی به نام 'ماست رامک' و قیمت '700' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' را ویرایش میکنم")]
        public void When()
        {
            _dto = GoodsFactory.CreateUpdateGoodsDto(_category.Id);

            _sut.Update(_goods.GoodsCode, _dto);
        }

        [Then("کالایی به نام 'ماست رامک' و قیمت '700' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' وجود دارد")]
        public void Then()
        {
            var expected = _dataContext.Goodss.FirstOrDefault();

            expected.Name.Should().Be(_dto.Name);
            expected.GoodsCode.Should().Be(_dto.GoodsCode);
            expected.MinInventory.Should().Be(_dto.MinInventory);
            expected.MaxInventory.Should().Be(_dto.MaxInventory);
            expected.Price.Should().Be(_dto.Price);
            expected.CategoryId.Should().Be(_category.Id);
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
