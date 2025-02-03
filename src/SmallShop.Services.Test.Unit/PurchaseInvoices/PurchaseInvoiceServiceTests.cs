using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Persistence.EF.PurchaseInvoices;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Services.PurchaseInvoices;
using SmallShop.Services.PurchaseInvoices.Contracts;
using SmallShop.Services.PurchaseInvoices.Exceptions;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using SmallShop.Test.Tools.PurchaseInvoices;
using Xunit;

namespace SmallShop.Services.Test.Unit.PurchaseInvoices
{
    public class PurchaseInvoiceServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly PurchaseInvoiceService _sut;
        private readonly PurchaseInvoiceRepository _repository;
        private readonly GoodsRepository _goodsRepository;
        private readonly CategoryRepository _categoryRepository;
        private PurchaseInvoice _purchaseInvoice;
        private Category _category;
        private Goods _goods;

        public PurchaseInvoiceServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPurchaseInvoiceRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new PurchaseInvoiceAppService(_repository, _unitOfWork, _goodsRepository);
        }

        [Fact]
        public  async Task Add_adds_purchaseInvoice_properly()
        {
            CreateCategoryWithGoods();
            AddPurchaseInvoiceDto dto = PurchaseInvoiceFactory.
                CreateAddPurchaseInvoiceDto(_goods.GoodsCode);

            await _sut.Add(dto);

            _dataContext.PurchaseInvoices.Should()
                .Contain(x => x.InvoiceNum == dto.InvoiceNum);
            _dataContext.PurchaseInvoices.Should()
                .Contain(x => x.SellerName == dto.SellerName);
            _dataContext.PurchaseInvoices.Should()
                .Contain(x => x.Price == dto.Price);
            _dataContext.PurchaseInvoices.Should()
                .Contain(x => x.Date == dto.Date);
            _dataContext.PurchaseInvoices.Should()
                .Contain(x => x.Count == dto.Count);
        }

        [Theory]
        [InlineData(40)]
        public async Task Add_throw_GoodsDoesNotExistException_when_goods_with_given_goodsCode_does_not_exist(int fakeCode)
        {
            AddPurchaseInvoiceDto dto = PurchaseInvoiceFactory.
                CreateAddPurchaseInvoiceDto(fakeCode);

            var expected = async () => await _sut.Add(dto);

            await expected.Should().ThrowExactlyAsync<GoodsDoesNotExistException>();
        }

        [Fact]
        public async Task Add_throw_InvoiceNumAlreadyExistException_When_given_invoiceNum_already_exist()
        {
            CreatePurchaseInvoice();
            AddPurchaseInvoiceDto dto = PurchaseInvoiceFactory.
                CreateAddPurchaseInvoiceDto(_purchaseInvoice.GoodsId);

            var expected = async () => await _sut.Add(dto);

            await expected.Should().ThrowExactlyAsync<InvoiceNumAlreadyExistException>();
        }

        [Fact]
        public async Task GetAll_return_all_purchaseInvoices_properly()
        {
            CreatePurchaseInvoice();

            var expected = await _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.SellerName ==
            _purchaseInvoice.SellerName);
            expected.Should().Contain(_ => _.Price == _purchaseInvoice.Price);
            expected.Should().Contain(_ => _.GoodsId ==
            _purchaseInvoice.GoodsId);
            expected.Should().Contain(_ => _.Count == _purchaseInvoice.Count);
            expected.Should().Contain(_ => _.Date == _purchaseInvoice.Date);
            expected.Should().Contain(_ => _.InvoiceNum ==
            _purchaseInvoice.InvoiceNum);
        }

        [Fact]
        public async Task Update_update_purchaseInvoice_properly()
        {
            CreatePurchaseInvoice();
            var dto = PurchaseInvoiceFactory.
                CreateUpdatePurchaseInvoiceDto(_purchaseInvoice.GoodsId);

            await _sut.Update(_purchaseInvoice.InvoiceNum, dto);

            _dataContext.PurchaseInvoices.Should().
                Contain(_=>_.SellerName==dto.SellerName);
            _dataContext.PurchaseInvoices.Should().
                Contain(_ => _.Date == dto.Date);
            _dataContext.PurchaseInvoices.Should().
                Contain(_ => _.GoodsId == dto.GoodsId);
            _dataContext.PurchaseInvoices.Should().
                Contain(_ => _.Count == dto.Count);
            _dataContext.PurchaseInvoices.Should().
                Contain(_ => _.Price == dto.Price);
        }

        [Theory]
        [InlineData(500, 2)]
        public async Task Update_throw_PurchaseInvoiceDoesNotExistException_when_given_invoiceNum_does_not_exist(int fakeId,int goodsId)
        {
            var dto = PurchaseInvoiceFactory.
                CreateUpdatePurchaseInvoiceDto(goodsId);

            var expected = async () => await _sut.Update(fakeId, dto);

            await expected.Should().ThrowExactlyAsync<PurchaseInvoiceDoesNotExistException>();
        }

        private void CreatePurchaseInvoice()
        {
            CreateCategoryWithGoods();
            _purchaseInvoice = PurchaseInvoiceFactory.
                CreatePurchaseInvoice(_goods.GoodsCode);
            _dataContext.Manipulate(_ => _
            .PurchaseInvoices.Add(_purchaseInvoice));
        }

        private void CreateCategoryWithGoods()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }
    }
}
