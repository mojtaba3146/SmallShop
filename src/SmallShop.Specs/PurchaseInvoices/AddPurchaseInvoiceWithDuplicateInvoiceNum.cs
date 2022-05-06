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
    [Scenario("ورود کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را وارد  کنم  ",
        InOrderTo = "کالایی به موجودی کالاها اضافه کنم"
    )]
    public class AddPurchaseInvoiceWithDuplicateInvoiceNum : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly PurchaseInvoiceService _sut;
        private readonly PurchaseInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private PurchaseInvoice _purchaseInvoice;
        private AddPurchaseInvoiceDto _dto;
        private Goods _goods;
        Action expected;

        public AddPurchaseInvoiceWithDuplicateInvoiceNum(ConfigurationFixture configuration) : base(configuration)
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

        [When("کالایی با نام ‘ماست رامک’ و تعداد ‘20’ و قیمت ‘ 400’ و کد کالای ‘10’ و تاریخ ’1401’ و نام فروشنده ‘سپهر’ وارد می شود")]
        public void When()
        {
            _dto = PurchaseInvoiceFactory.CreateAddPurchaseInvoiceDto(_goods.GoodsCode);

            expected = () => _sut.Add(_dto);
        }

        [Then("تنها یک سند ورود کالا با کد فاکتور '1' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.PurchaseInvoices.Where(
                _=>_.InvoiceNum == _dto.InvoiceNum)
                .Should().HaveCount(1);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }


        private void CreatePurchaseInvoice()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            _goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
            _purchaseInvoice = PurchaseInvoiceFactory.
                CreatePurchaseInvoice(_goods.GoodsCode);
            _dataContext.Manipulate(_ => _
            .PurchaseInvoices.Add(_purchaseInvoice));
        }
    }  
}
