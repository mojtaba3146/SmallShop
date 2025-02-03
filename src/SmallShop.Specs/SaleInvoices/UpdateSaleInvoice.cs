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
    [Scenario("ویرایش سند خروج کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " ویرایش سند خروج کالا  ",
       InOrderTo = "سند خروج کالا ها را ویرایش کنم"
   )]
    public class UpdateSaleInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SaleInvoiceService _sut;
        private readonly SaleInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private SaleInvoice _saleInvoice;
        private UpdateSaleInvoiceDto _dto;
        private Goods _goods;
        public UpdateSaleInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSaleInvoiceRepository(_dataContext);
            _goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new SaleInvoiceAppService(_repository, _unitOfWork, _goodsRepository);
        }

        [Given("سند خروج کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘2’ و قیمت ‘500’ و نام خریدار ‘سپهر’ در سیستم وجود دارد")]
        public void Given()
        {
            CreateSaleInvoice();
        }

        [When("درخواست ویرایش سند خروج کالا را می دهم")]
        public async Task When()
        {
            _dto = SaleInvoiceFactory.
                CreateSaleInvoiceUpdateDto(_goods.GoodsCode);

            await _sut.Update(_saleInvoice.InvoiceNum, _dto);
        }

        [Then("سند خروج کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘2’ و قیمت ‘500’ و نام خریدار ‘امین’ در سیستم وجود دارد")]
        public async Task Then()
        {
            var expected = await _dataContext.SaleInvoices.FirstOrDefaultAsync();


            expected!.Price.Should().Be(_dto.Price);
            expected.Count.Should().Be(_dto.Count);
            expected.BuyerName.Should().Be(_dto.BuyerName);
            expected.GoodsId.Should().Be(_dto.GoodsId);
            expected.Date.Should().Be(_dto.Date);
        }

        [Fact]
        public async Task Run()
        {
            Given();
            await When();
            await Then();
        }


        private void CreateSaleInvoice()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            _goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
            _saleInvoice = SaleInvoiceFactory.
                CreateSaleInvoice(_goods.GoodsCode);
            _dataContext.Manipulate(_ => _
            .SaleInvoices.Add(_saleInvoice));
        }
    }
}
