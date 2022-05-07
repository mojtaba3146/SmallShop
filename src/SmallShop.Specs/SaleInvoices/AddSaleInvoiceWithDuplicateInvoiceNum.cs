using FluentAssertions;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class AddSaleInvoiceWithDuplicateInvoiceNum : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SaleInvoiceService _sut;
        private readonly SaleInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private AddSaleInvoiceDto _dto;
        private SaleInvoice _saleInvoice;
        private Category _category;
        private Goods _goods;
        Action expected;
        public AddSaleInvoiceWithDuplicateInvoiceNum(ConfigurationFixture configuration) : base(configuration)
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

        [When("کالایی با نام ‘ماست رامک’ و تعداد ‘2’ و قیمت ‘ 500’ و کد کالای ‘10’ و تاریخ ’1401’ و نام خریدار ‘سپهر’ خارج می شود")]
        public void When()
        {
            _dto = SaleInvoiceFactory.CreateAddSaleInvoiceDto(_goods.GoodsCode);

           expected = () => _sut.Add(_dto);
        }

        [Then("تنها یک سند خروج کالا با کد فاکتور '1' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.SaleInvoices.Where(
                _ => _.InvoiceNum == _dto.InvoiceNum)
                .Should().HaveCount(1);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
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
