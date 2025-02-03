using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Persistence.EF.SaleInvoices;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.SaleInvoices;
using SmallShop.Services.SaleInvoices.Contracts;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using SmallShop.Test.Tools.SaleInvoices;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.SaleInvoices
{
    [Scenario("فروش کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را به فروش رسانم  ",
        InOrderTo = "کالا ها را بفروشم "
    )]
    public class AddSaleInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SaleInvoiceService _sut;
        private readonly SaleInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private AddSaleInvoiceDto _dto;
        private Category _category;
        private Goods _goods;
        public AddSaleInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSaleInvoiceRepository(_dataContext);
            _goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new SaleInvoiceAppService(_repository, _unitOfWork, _goodsRepository);
        }

        [Given("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و موجودی '5' در دسته 'لبنیات' وجود دارد ")]
        public void Given()
        {
            CreateGoods();
        }

        [And("هیچ سند خروج کالایی در لیست سند خروج کالا وجود ندارد")]
        public void GivenAnd()
        {

        }

        [When("کالایی با نام ‘ماست رامک’ و تعداد ‘2’ و قیمت ‘ 500’ و کد کالای ‘10’ و تاریخ ’1401’ و نام خریدار ‘سپهر’ خارج می شود")]
        public async Task When()
        {
            _dto = SaleInvoiceFactory.CreateAddSaleInvoiceDto(_goods.GoodsCode);

            await _sut.Add(_dto);
        }

        [Then("سند خروج کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘2’ و قیمت ‘500’ و نام خریدار ‘سپهر’ در سیستم وجود دارد")]
        public async Task Then()
        {
            var expected = await _dataContext.SaleInvoices.FirstOrDefaultAsync();

            expected.InvoiceNum.Should().Be(_dto.InvoiceNum);
            expected.Price.Should().Be(_dto.Price);
            expected.Count.Should().Be(_dto.Count);
            expected.BuyerName.Should().Be(_dto.BuyerName);
            expected.GoodsId.Should().Be(_dto.GoodsId);
        }

        [And("کالایی با نام ‘ماست رامک’ و موجودی ‘23’ و کد کالای ‘10’ و تعداد فروش '2' باید در فهرست کالا ها وجود داشته باشد")]
        public void ThenAnd()
        {
            _goods.GoodsInventory.Should().Be(_goods.GoodsInventory);
            _goods.SellCount.Should().Be(_goods.SellCount);
        }

        [Fact]
        public async Task Run()
        {
            Given();
            GivenAnd();
            await When();
            await Then();
            ThenAnd();
        }

        private void CreateGoods()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }
    }
}
