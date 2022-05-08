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
using System.Linq;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.SaleInvoices
{
    [Scenario("مشاهده سند خروج کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = "  مشاهده سند خروج کالا  ",
       InOrderTo = "سند خروج کالا ها را مشاهده کنم"
   )]
    public class GetAllSaleInvoices : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SaleInvoiceService _sut;
        private readonly SaleInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private SaleInvoice _saleInvoice;
        private Goods _goods;
        public GetAllSaleInvoices(ConfigurationFixture configuration) : base(configuration)
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

        [When("درخواست مشاهده سند خروج کالا ها را می دهم")]
        public void When()
        {
            _sut.GetAll();
        }

        [Then("")]
        public void Then()
        {
            var expected = _dataContext.SaleInvoices.ToList();

            expected.Should().HaveCount(1);
            expected.Should().Contain(x => x.InvoiceNum ==
            _saleInvoice.InvoiceNum);
            expected.Should().Contain(x => x.Price ==
            _saleInvoice.Price);
            expected.Should().Contain(x => x.Date ==
            _saleInvoice.Date);
            expected.Should().Contain(x => x.GoodsId ==
            _saleInvoice.GoodsId);
            expected.Should().Contain(x => x.BuyerName ==
            _saleInvoice.BuyerName);
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
