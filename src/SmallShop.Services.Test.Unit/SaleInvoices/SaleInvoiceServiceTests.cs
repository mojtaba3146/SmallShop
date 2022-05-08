using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Persistence.EF.SaleInvoices;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Services.SaleInvoices;
using SmallShop.Services.SaleInvoices.Contracts;
using SmallShop.Services.SaleInvoices.Exceptions;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using SmallShop.Test.Tools.SaleInvoices;
using System;
using Xunit;

namespace SmallShop.Services.Test.Unit.SaleInvoices
{
    public class SaleInvoiceServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly SaleInvoiceService _sut;
        private readonly SaleInvoiceRepository _repository;
        private readonly GoodsRepository _goodsRepository;
        private readonly CategoryRepository _categoryRepository;
        private SaleInvoice _saleInvoice;
        private Category _category;
        private Goods _goods;

        public SaleInvoiceServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSaleInvoiceRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new SaleInvoiceAppService(_repository, _unitOfWork, _goodsRepository);
        }

        [Fact]
        public void Add_adds_saleInvoice_properly()
        {
            CreateCategoryWithGoods();
            AddSaleInvoiceDto dto = SaleInvoiceFactory.
                CreateAddSaleInvoiceDto(_goods.GoodsCode);

            _sut.Add(dto);

            _dataContext.SaleInvoices.Should()
                .Contain(x => x.InvoiceNum == dto.InvoiceNum);
            _dataContext.SaleInvoices.Should()
                .Contain(x => x.BuyerName == dto.BuyerName);
            _dataContext.SaleInvoices.Should()
                .Contain(x => x.Price == dto.Price);
            _dataContext.SaleInvoices.Should()
                .Contain(x => x.Date == dto.Date);
            _dataContext.SaleInvoices.Should()
                .Contain(x => x.Count == dto.Count);
        }

        [Theory]
        [InlineData(40)]
        public void Add_throw_GoodsDoesNotExistException_when_goods_with_given_goodsCode_does_not_exist(int fakeCode)
        {
            AddSaleInvoiceDto dto = SaleInvoiceFactory.
                CreateAddSaleInvoiceDto(fakeCode);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<GoodsDoesNotExistException>();
        }

        [Fact]
        public void Add_throw_InvoiceNumAlreadyExistException_When_given_invoiceNum_already_exist()
        {
            CreateSaleInvoice();
            AddSaleInvoiceDto dto = SaleInvoiceFactory.
                CreateAddSaleInvoiceDto(_saleInvoice.GoodsId);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<InvoiceNumAlreadyExistException>();
        }

        [Fact]
        public void GetAll_return_all_saleInvoices_properly()
        {
            CreateSaleInvoice();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.BuyerName ==
            _saleInvoice.BuyerName);
            expected.Should().Contain(_ => _.Price == _saleInvoice.Price);
            expected.Should().Contain(_ => _.GoodsId ==
            _saleInvoice.GoodsId);
            expected.Should().Contain(_ => _.Count == _saleInvoice.Count);
            expected.Should().Contain(_ => _.Date == _saleInvoice.Date);
            expected.Should().Contain(_ => _.InvoiceNum ==
            _saleInvoice.InvoiceNum);
        }

        [Fact]
        public void Update_update_SaleInvoice_properly()
        {
            CreateSaleInvoice();
            var dto = SaleInvoiceFactory.
                CreateSaleInvoiceUpdateDto(_goods.GoodsCode);

            _sut.Update(_saleInvoice.InvoiceNum, dto);

            _dataContext.SaleInvoices.Should().
                Contain(_ => _.BuyerName == dto.BuyerName);
            _dataContext.SaleInvoices.Should().
                Contain(_ => _.Date == dto.Date);
            _dataContext.SaleInvoices.Should().
                Contain(_ => _.GoodsId == dto.GoodsId);
            _dataContext.SaleInvoices.Should().
                Contain(_ => _.Count == dto.Count);
            _dataContext.SaleInvoices.Should().
                Contain(_ => _.Price == dto.Price);
        }

        [Theory]
        [InlineData(500, 2)]
        public void Update_throw_SaleInvoiceDoesNotExistException_when_given_invoiceNum_does_not_exist(int fakeId, int goodsId)
        {
            var dto = SaleInvoiceFactory.
                CreateSaleInvoiceUpdateDto(goodsId);

            Action expected = () => _sut.Update(fakeId, dto);

            expected.Should().ThrowExactly<SaleInvoiceDoesNotExistException>();
        }

        private void CreateSaleInvoice()
        {
            CreateCategoryWithGoods();
            _saleInvoice = SaleInvoiceFactory.
                CreateSaleInvoice(_goods.GoodsCode);
            _dataContext.Manipulate(_ => _
            .SaleInvoices.Add(_saleInvoice));
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
