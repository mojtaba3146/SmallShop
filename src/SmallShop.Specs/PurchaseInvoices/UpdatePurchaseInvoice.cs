using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Persistence.EF.PurchaseInvoices;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.PurchaseInvoices;
using SmallShop.Services.PurchaseInvoices.Contracts;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using SmallShop.Test.Tools.PurchaseInvoices;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.PurchaseInvoices
{
    [Scenario("ویرایش سند ورود کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " ویرایش سند ورود کالا  ",
       InOrderTo = "سند ورود کالا ها را ویرایش کنم"
   )]
    public class UpdatePurchaseInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly PurchaseInvoiceService _sut;
        private readonly PurchaseInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private PurchaseInvoice _purchaseInvoice;
        private UpdatePurchaseInvoiceDto _dto;
        public UpdatePurchaseInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPurchaseInvoiceRepository(_dataContext);
            _goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_repository, _unitOfWork, _goodsRepository);
        }

        [Given("سند ورود کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘20’ و قیمت ‘400’ و نام فروشنده ‘سپهر’ در سیستم وجود دارد")]
        public void Given()
        {
            CreatePurchaseInvoice();
        }

        [When("درخواست ویرایش سند ورود کالا را می دهم")]
        public async Task When()
        {
            _dto = PurchaseInvoiceFactory.
                CreateUpdatePurchaseInvoiceDto(_purchaseInvoice.GoodsId);

            await _sut.Update(_purchaseInvoice.InvoiceNum, _dto);
        }

        [Then(" سند ورود کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘20’ و قیمت ‘400’ و نام فروشنده ‘امین’ در سیستم وجود دارد ")]
        public async Task Then()
        {
            var expected = await _dataContext.PurchaseInvoices.FirstOrDefaultAsync();

            
            expected!.Price.Should().Be(_dto.Price);
            expected.Count.Should().Be(_dto.Count);
            expected.SellerName.Should().Be(_dto.SellerName);
            expected.GoodsId.Should().Be(_dto.GoodsId);
            expected.Date.Should().Be(_dto.Date);
        }

        [Fact]
        public async void Run()
        {
            Given();
            await When();
            await Then();
        }

        private void CreatePurchaseInvoice()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            _purchaseInvoice = PurchaseInvoiceFactory.
                CreatePurchaseInvoice(goods.GoodsCode);
            _dataContext.Manipulate(_ => _
            .PurchaseInvoices.Add(_purchaseInvoice));
        }
    }
}
