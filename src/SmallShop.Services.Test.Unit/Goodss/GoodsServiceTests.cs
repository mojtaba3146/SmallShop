using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Services.Goodss.Exceptions;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmallShop.Services.Test.Unit.Goodss
{
    public class GoodsServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Goods _goods, _goodsTwo;

        public GoodsServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            AddGoodsDto dto = GoodsFactory.CreateAddGoodsDto(category.Id);

            _sut.Add(dto);

            _dataContext.Goodss.Should()
                .Contain(x => x.GoodsCode == dto.GoodsCode);
            _dataContext.Goodss.Should()
                .Contain(x => x.Name == dto.Name);
            _dataContext.Goodss.Should()
                .Contain(x => x.Price == dto.Price);
            _dataContext.Goodss.Should()
                .Contain(x => x.MinInventory == dto.MinInventory);
            _dataContext.Goodss.Should()
                .Contain(x => x.MaxInventory == dto.MaxInventory);
            _dataContext.Goodss.Should()
                .Contain(x => x.CategoryId == dto.CategoryId);
        }

        [Fact]
        public void Add_throws_GoodsNameIsDuplicatedException_when_goods_name_already_exists_in_category()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            AddGoodsDto dto = GoodsFactory.CreateAddGoodsDto(category.Id);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<GoodsNameIsDuplicatedException>();
        }

        [Theory]
        [InlineData(40, 20)]
        public void Add_throw_CategoryNotFoundException_when_category_with_given_id_not_exists(int fakeCategoryId, int categoryID)
        {
            var dto = GoodsFactory.CreateAddGoodsDto(categoryID);
            dto.CategoryId = fakeCategoryId;

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void GetAll_return_all_goods()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == goods.Name);
            expected.Should().Contain(_ => _.Price == goods.Price);
            expected.Should().Contain(_ => _.GoodsCode == goods.GoodsCode);
            expected.Should().Contain(_ => _.MinInventory == goods.MinInventory);
            expected.Should().Contain(_ => _.MaxInventory == goods.MaxInventory);
            expected.Should().Contain(_ => _.CategoryId == goods.CategoryId);
        }

        [Fact]
        public void Update_update_goods_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            var dto = GoodsFactory.CreateUpdateGoodsDto(category.Id);

            _sut.Update(goods.GoodsCode, dto);

            _dataContext.Goodss.Should().Contain(_ => _.Name == dto.Name);
            _dataContext.Goodss.Should().Contain(_ => _.Price == dto.Price);
            _dataContext.Goodss.Should().Contain(_ => _.MinInventory == dto.MinInventory);
            _dataContext.Goodss.Should().Contain(_ => _.MaxInventory == dto.MaxInventory);
            _dataContext.Goodss.Should().Contain(_ => _.CategoryId == dto.CategoryId);
            _dataContext.Goodss.Should().Contain(_ => _.GoodsCode == dto.GoodsCode);
        }

        [Theory]
        [InlineData(500, 2)]
        public void Update_throw_GoodsDoesNotExistException_when_given_id_does_not_exist(int fakegoodsCode, int categoryId)
        {
            var dto = GoodsFactory.CreateUpdateGoodsDto(categoryId);

            Action expected = () => _sut.Update(fakegoodsCode, dto);

            expected.Should().ThrowExactly<GoodsDoesNotExistException>();
        }

        [Theory]
        [InlineData(40, 20)]
        public void Update_throw_CategoryNotFoundException_when_category_with_given_id_not_exists(int fakeCategoryId, int categoryID)
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            var dto = GoodsFactory.CreateUpdateGoodsDto(category.Id);
            dto.CategoryId = fakeCategoryId;

            Action expected = () => _sut.Update(goods.GoodsCode, dto);

            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }

        [Fact]
        public void Update_throws_GoodsNameIsDuplicatedException_when_goods_name_already_exists_in_category()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
            var goodss = GoodsFactory.CreateGoods(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goodss));
            UpdateGoodsDto dto = GoodsFactory.CreateUpdateGoods(category.Id);

            Action expected = () => _sut.Update(goods.GoodsCode, dto);

            expected.Should().ThrowExactly<GoodsNameIsDuplicatedException>();
        }

        [Fact]
        public void Delete_delete_goods_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));

            _sut.Delete(goods.GoodsCode);

            _dataContext.Goodss.Should()
                .NotContain(_ => _.GoodsCode == goods.GoodsCode);
        }

        [Theory]
        [InlineData(500)]
        public void Delete_throw_GoodsDoesNotExistException_when_goods_with_given_goodscode_does_not_exist(int fakeId)
        {
            Action expected = () => _sut.Delete(fakeId);

            expected.Should().ThrowExactly<GoodsDoesNotExistException>();
        }

        [Fact]
        public void GetBestSellerGoods_return_goods_that_sellNum_is_max()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id).WithSellCount(5)
                .WithGoodsInventory(21).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
            _goodsTwo = new GoodsBuilder(_category.Id).WithName("ماست میهن")
                .WithGoodsCode(20).WithGoodsInventory(22)
                .WithSellCount(11).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goodsTwo));

            var expected = _sut.GetBestSellerGoods();

            expected.Name.Should().Be(_goodsTwo.Name);
            expected.GoodsCode.Should().Be(_goodsTwo.GoodsCode);
        }

        [Fact]
        public void GetAllMinInventory_return_goods_with_goodsInventory_less_than_minInventory()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id)
                .WithGoodsInventory(15).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));

            var expected = _sut.GetAllMinInventory();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.CategoryId == _goods.CategoryId);
        }

        [Fact]
        public void GetAllMaxInventory_return_goods_with_goodsInventory_more_than_maxInventory()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id)
                .WithGoodsInventory(40).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));

            var expected = _sut.GetAllMaxInventory();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.CategoryId == _goods.CategoryId);
        }
    }
}
