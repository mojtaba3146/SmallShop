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
    [Scenario("ورود کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا را وارد  کنم  ",
        InOrderTo = "کالایی به موجودی کالاها اضافه کنم"
    )]
    public class AddPurchaseInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly PurchaseInvoiceService _sut;
        private readonly PurchaseInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsRepository _goodsRepository;
        private AddPurchaseInvoiceDto _dto;
        private Category _category;
        private Goods _goods;
        public AddPurchaseInvoice(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPurchaseInvoiceRepository(_dataContext);
            _goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_repository, _unitOfWork,_goodsRepository);
        }

        [Given("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' در دسته 'لبنیات' وجود دارد ")]
        public void Given()
        {
            CreateGoods();
        }

        [And("هیچ سند ورود کالایی در لیست سند ورود کالا وجود ندارد")]
        public void GivenAnd()
        {

        }

        [When("کالایی با نام ‘ماست رامک’ و تعداد ‘20’ و قیمت ‘ 400’ و کد کالای ‘10’ و تاریخ ’1401’ و نام فروشنده ‘سپهر’ وارد می شود")]
        public async Task When()
        {
            _dto = PurchaseInvoiceFactory.CreateAddPurchaseInvoiceDto(_goods.GoodsCode);

            await _sut.Add(_dto);
        }

        [Then("سند ورود کالایی با کد فاکتور ‘1’ و کد کالای ‘10’ و تاریخ '1401' و تعداد کالای ‘20’ و قیمت ‘400’ و نام فروشنده ‘سپهر’ در سیستم وجود دارد")]
        public async Task Then()
        {
            var expected = await _dataContext.PurchaseInvoices
                .FirstOrDefaultAsync();

            expected!.InvoiceNum.Should().Be(_dto.InvoiceNum);
            expected.Price.Should().Be(_dto.Price);
            expected.Count.Should().Be(_dto.Count);
            expected.SellerName.Should().Be(_dto.SellerName);
            expected.GoodsId.Should().Be(_dto.GoodsId);
        }

        [And("کالایی با نام ‘ماست رامک’ و موجودی ‘25’ و کد کالای ‘10’ باید در فهرست کالا ها وجود داشته باشد")]
        public void ThenAnd()
        {
            _goods.GoodsInventory.Should().Be(_goods.GoodsInventory);
        }

        [Fact]
        public async void Run()
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
