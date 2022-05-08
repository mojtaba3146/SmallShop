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
    [Scenario("مشاهده کالای با موجودی بیشتر از حداکثر موجودی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "  مشاهده کالا هایی با موجودی بیشتر از حداقل موجودی ",
        InOrderTo = "از آن کالا ها سفارش ندهم"
    )]
    public class GetGoodsWithMaxInven : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Goods _goods;
        public GetGoodsWithMaxInven(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' و موجودی '40' در دسته 'لبنیات' وجود دارد")]
        public void Given()
        {
            CreateGoods();
        }

        [When("درخواست مشاهده کالایی با موجودی بیشتر از حداکثر موجودی را دارم")]
        public void When()
        {
            _sut.GetAllMaxInventory();
        }

        [Then("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' و موجودی '40' در دسته 'لبنیات' نمایش داده می شود")]
        public void Then()
        {
            var expected = _dataContext.Goodss.ToList();

            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }

        private void CreateGoods()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id)
                .WithGoodsInventory(40).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }
    }
}
