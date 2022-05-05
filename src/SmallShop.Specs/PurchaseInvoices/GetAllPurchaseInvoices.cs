using FluentAssertions;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.PurchaseInvoices
{
    [Scenario("مشاهده سند ورود کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " درخواست مشاهده سند ورود کالا  ",
       InOrderTo = "سند ورود کالا ها را مشاهده کنم"
   )]
    public class GetAllPurchaseInvoices : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly PurchaseInvoiceService _sut;
        private readonly PurchaseInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private PurchaseInvoice _purchaseInvoice;
        public GetAllPurchaseInvoices(ConfigurationFixture configuration) : base(configuration)
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
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            _purchaseInvoice = PurchaseInvoiceFactory.
                CreatePurchaseInvoice(goods.GoodsCode);
            _dataContext.Manipulate(_ => _
            .PurchaseInvoices.Add(_purchaseInvoice));
        }

        [When("درخواست مشاهده سند ورود کالا ها را می دهم")]
        public void When()
        {
            _sut.GetAll();
        }

        [Then("سند ورود کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘20’ و قیمت ‘400’ و نام فروشنده ‘سپهر’ نمایش داده میشود")]
        public void Then()
        {
            var expected = _dataContext.PurchaseInvoices.ToList();

            expected.Should().HaveCount(1);
            expected.Should().Contain(x=>x.InvoiceNum==
            _purchaseInvoice.InvoiceNum);
            expected.Should().Contain(x => x.Price ==
            _purchaseInvoice.Price);
            expected.Should().Contain(x => x.Date ==
            _purchaseInvoice.Date);
            expected.Should().Contain(x => x.GoodsId ==
            _purchaseInvoice.GoodsId);
            expected.Should().Contain(x => x.SellerName ==
            _purchaseInvoice.SellerName);
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
