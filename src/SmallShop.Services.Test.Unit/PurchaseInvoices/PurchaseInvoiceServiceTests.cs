using FluentAssertions;
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
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using SmallShop.Test.Tools.PurchaseInvoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void Add_adds_purchaseInvoice_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            AddPurchaseInvoiceDto dto = PurchaseInvoiceFactory.
                CreateAddPurchaseInvoiceDto(goods.GoodsCode);

            _sut.Add(dto);

            _dataContext.PurchaseInvoices.Should()
                .Contain(x=>x.InvoiceNum==dto.InvoiceNum);
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
        public void Add_throw_GoodsDoesNotExistException_when_goods_with_given_goodsCode_does_not_exist(int fakeCode)
        {
            AddPurchaseInvoiceDto dto = PurchaseInvoiceFactory.
                CreateAddPurchaseInvoiceDto(fakeCode);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<GoodsDoesNotExistException>();
        }
    }
}
