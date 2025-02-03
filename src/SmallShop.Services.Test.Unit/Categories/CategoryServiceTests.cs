using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Categories.Exceptions;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using Xunit;

namespace SmallShop.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly CategoryService _sut;
        private Category _category;

        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _sut = CategoryFactory.CreateService(_dataContext);
            _category = new Category();
        }

        [Fact]
        public async Task Add_adds_category_properly()
        {
            AddCategoryDto dto = CategoryFactory.CreateAddCategoryDto("لبنیات");

            await _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(x => x.Title == dto.Title);
        }
        [Fact]
        public async Task Add_throw_TitleAlreadyExistException_when_given_title_alreadyExist()
        {
            CreateCategory("لبنیات");
            AddCategoryDto dto = CategoryFactory.CreateAddCategoryDto("لبنیات");

            var expected = async () => await _sut.Add(dto);

            await expected.Should().ThrowExactlyAsync<TitleAlreadyExistException>();
        }

        [Fact]
        public async Task Update_update_category_properly()
        {
            CreateCategory("لبنیات");
            var dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            await _sut.Update(_category.Id, dto);

            _dataContext.Categories.Should()
                .Contain(x => x.Title == dto.Title);
        }

        [Fact]
        public async Task Update_throw_TitleAlreadyExistException_when_given_title_alreadyExist()
        {
            CreateCategory("لبنیات");
            CreateCategory("خشکبار");
            var dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            var expected = async () => await _sut.Update(_category.Id, dto);

            await expected.Should().ThrowExactlyAsync<TitleAlreadyExistException>();
        }

        [Theory]
        [InlineData(500)]
        public async Task Update_throw_CategoryWithGivenIdDoesNotExist_when_given_id_does_not_exist(int fakeCategoryId)
        {
            var dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            var expected = async () => await _sut.Update(fakeCategoryId, dto);

            await expected.Should().ThrowExactlyAsync<CategoryWithGivenIdDoesNotExist>();
        }

        [Fact]
        public async Task GetAll_return_all_categories_properly()
        {
            CreateCategory("لبنیات");

            var expected = await _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Title == _category.Title);
        }

        [Fact]
        public async Task Delete_delete_category_properly()
        {
            CreateCategory("لبنیات");

            await _sut.Delete(_category.Id);

            _dataContext.Categories.Should()
                .NotContain(_ => _.Title == _category.Title);
        }

        [Theory]
        [InlineData(500)]
        public async Task Delete_throw_CategoryWithGivenIdDoesNotExist_when_given_id_does_not_exist(int fakeCategoryId)
        {
            var expected = async () => await _sut.Delete(fakeCategoryId);

            await expected.Should().ThrowExactlyAsync<CategoryWithGivenIdDoesNotExist>();
        }

        [Fact]
        public async Task Delete_throw_GoodsExistInCategoryException_when_goods_exist_in_category()
        {
            CreateCategory("لبنیات");
            CreateGoods();

            var expected = async () => await _sut.Delete(_category.Id);

            await expected.Should().ThrowExactlyAsync<GoodsExistInCategoryException>();
        }

        [Fact]
        public async Task AddSeed_adds_seed_category_properly()
        {
            AddCategoryDto dto = CategoryFactory.CreateAddCategoryDto("dummyCategory");

            await _sut.AddSeed(dto);

            _dataContext.Categories.Should()
                .Contain(x => x.Title == dto.Title);
        }

        [Fact]
        public async Task AddSeed_return_category_id_properly()
        {
            var id = CreateCategory("dummyCategory");
            AddCategoryDto dto = CategoryFactory.CreateAddCategoryDto("dummyCategory");

            var expected = await _sut.AddSeed(dto);

            expected.Should().Be(id);
        }


        private int CreateCategory(string categoryTitle)
        {
            _category = CategoryFactory.CreateCategory(categoryTitle);
            _dataContext.Manipulate(_ => _.Categories.Add(_category));

            return _category.Id;
        }
        private void CreateGoods()
        {
            var goods = GoodsFactory.CreateGoodsWithCategory(_category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));
        }

    }
}
