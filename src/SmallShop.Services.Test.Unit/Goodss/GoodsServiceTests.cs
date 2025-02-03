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
using System.Linq;
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
        public async Task Add_adds_goods_properly()
        {
            CreateCategory("لبنیات");
            AddGoodsDto dto = GoodsFactory.CreateAddGoodsDto(_category.Id);

            await _sut.Add(dto);

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
        public async Task Add_throws_GoodsNameIsDuplicatedException_when_goods_name_already_exists_in_category()
        {
            CreateCategory("لبنیات");
            CreateGoods();
            AddGoodsDto dto = GoodsFactory.CreateAddGoodsDto(_category.Id);

            var expected = async () => await _sut.Add(dto);

            await expected.Should().ThrowExactlyAsync<GoodsNameIsDuplicatedException>();
        }

        [Theory]
        [InlineData(40, 20)]
        public async Task Add_throw_CategoryNotFoundException_when_category_with_given_id_not_exists(int fakeCategoryId, int categoryID)
        {
            var dto = GoodsFactory.CreateAddGoodsDto(categoryID);
            dto.CategoryId = fakeCategoryId;

            var expected = async () => await _sut.Add(dto);

            await expected.Should().ThrowExactlyAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task GetAll_return_all_goods()
        {
            CreateCategory("لبنیات");
            CreateGoods();

            var expected = await _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.Price == _goods.Price);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.MinInventory == _goods.MinInventory);
            expected.Should().Contain(_ => _.MaxInventory == _goods.MaxInventory);
            expected.Should().Contain(_ => _.CategoryId == _goods.CategoryId);
        }

        [Fact]
        public async Task Update_update_goods_properly()
        {
            CreateCategory("لبنیات");
            CreateGoods();
            var dto = GoodsFactory.CreateUpdateGoodsDto(_category.Id);

            await _sut.Update(_goods.GoodsCode, dto);

            _dataContext.Goodss.Should().Contain(_ => _.Name == dto.Name);
            _dataContext.Goodss.Should().Contain(_ => _.Price == dto.Price);
            _dataContext.Goodss.Should().Contain(_ => _.MinInventory == dto.MinInventory);
            _dataContext.Goodss.Should().Contain(_ => _.MaxInventory == dto.MaxInventory);
            _dataContext.Goodss.Should().Contain(_ => _.CategoryId == dto.CategoryId);
            _dataContext.Goodss.Should().Contain(_ => _.GoodsCode == dto.GoodsCode);
        }

        [Theory]
        [InlineData(500, 2)]
        public async Task Update_throw_GoodsDoesNotExistException_when_given_id_does_not_exist(int fakegoodsCode, int categoryId)
        {
            var dto = GoodsFactory.CreateUpdateGoodsDto(categoryId);

            var expected = async () => await _sut.Update(fakegoodsCode, dto);

            await expected.Should().ThrowExactlyAsync<GoodsDoesNotExistException>();
        }

        [Theory]
        [InlineData(40, 20)]
        public async Task Update_throw_CategoryNotFoundException_when_category_with_given_id_not_exists(int fakeCategoryId, int categoryID)
        {
            CreateCategory("لبنیات");
            CreateGoods();
            var dto = GoodsFactory.CreateUpdateGoodsDto(_category.Id);
            dto.CategoryId = fakeCategoryId;

            var expected = async () => await _sut.Update(_goods.GoodsCode, dto);

            await expected.Should().ThrowExactlyAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task Update_throws_GoodsNameIsDuplicatedException_when_goods_name_already_exists_in_category()
        {
            CreateCategory("لبنیات");
            CreateGoods();
            CreateSecondGoods();
            UpdateGoodsDto dto = GoodsFactory.CreateUpdateGoods(_category.Id);

            var expected = async () => await _sut.Update(_goods.GoodsCode, dto);

            await expected.Should().ThrowExactlyAsync<GoodsNameIsDuplicatedException>();
        }

        [Fact]
        public async Task Delete_delete_goods_properly()
        {
            CreateCategory("لبنیات");
            CreateGoods();

            await _sut.Delete(_goods.GoodsCode);

            _dataContext.Goodss.Should()
                .NotContain(_ => _.GoodsCode == _goods.GoodsCode);
        }

        [Theory]
        [InlineData(500)]
        public async Task Delete_throw_GoodsDoesNotExistException_when_goods_with_given_goodscode_does_not_exist(int fakeId)
        {
            var expected = async () => await _sut.Delete(fakeId);

            await expected.Should().ThrowExactlyAsync<GoodsDoesNotExistException>();
        }

        [Fact]
        public async Task GetBestSellerGoods_return_goods_that_sellNum_is_max()
        {
            CreateCategory("لبنیات");
            CreateTwoGoods();

            var expected = await _sut.GetBestSellerGoods();

            expected!.Name.Should().Be(_goodsTwo.Name);
            expected.GoodsCode.Should().Be(_goodsTwo.GoodsCode);
        }

        [Fact]
        public async Task GetBestSellerGoodsInEchCategory_return_goods_that_sellNum_is_max_in_each_category()
        {
            CreateCategory("لبنیات");
            CreateCategory("نوشیدنی");
            CreateTwoGoodsWithDiffrentCategory();

            var expected = await _sut.GetBestSellerGoodsInEchCategory();

            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.Name == _goodsTwo.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goodsTwo.GoodsCode);
        }

        [Fact]
        public async Task GetAllMinInventory_return_goods_with_goodsInventory_less_than_minInventory()
        {
            CreateCategory("لبنیات");
            _goods = new GoodsBuilder(_category.Id)
                .WithGoodsInventory(15).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));

            var expected = await _sut.GetAllMinInventory();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.CategoryId == _goods.CategoryId);
        }

        [Fact]
        public  async Task GetAllMaxInventory_return_goods_with_goodsInventory_more_than_maxInventory()
        {
            CreateCategory("لبنیات");
            _goods = new GoodsBuilder(_category.Id)
                .WithGoodsInventory(40).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));

            var expected = await _sut.GetAllMaxInventory();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _goods.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goods.GoodsCode);
            expected.Should().Contain(_ => _.CategoryId == _goods.CategoryId);
        }

        private void CreateCategory(string title)
        {
            _category = CategoryFactory.CreateCategory(title);
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        private void CreateGoods()
        {
            _goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }

        private void CreateSecondGoods()
        {
            _goodsTwo = GoodsFactory.CreateGoods(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(_goodsTwo));
        }

        private void CreateTwoGoods()
        {
            _goods = new GoodsBuilder(_category.Id).WithSellCount(5)
                .WithGoodsInventory(21).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
            _goodsTwo = new GoodsBuilder(_category.Id).WithName("ماست میهن")
                .WithGoodsCode(20).WithGoodsInventory(22)
                .WithSellCount(11).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goodsTwo));
        }

        private void CreateTwoGoodsWithDiffrentCategory()
        {
            var categoryIdLabaniat = _dataContext.Categories
                .Where(x => x.Title == "لبنیات")
                .Select(x => x.Id).First();
            var categoryIdNoshidani = _dataContext.Categories
                .Where(x => x.Title == "نوشیدنی")
                .Select(x => x.Id).First();
            _goods = new GoodsBuilder(categoryIdLabaniat).WithSellCount(5)
               .WithGoodsInventory(21).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
            _goodsTwo = new GoodsBuilder(categoryIdNoshidani).WithGoodsCode(12)
                .WithSellCount(15).WithName("نوشابه کولا")
                .WithGoodsInventory(21).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goodsTwo));
        }
    }
}
