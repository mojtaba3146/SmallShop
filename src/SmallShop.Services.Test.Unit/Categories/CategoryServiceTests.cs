using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Services.Categories;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Categories.Exceptions;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SmallShop.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;

        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto =CategoryFactory.CreateAddCategoryDto("لبنیات");

            _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(x => x.Title == dto.Title);
        }
        [Fact]
        public void Add_throw_TitleAlreadyExistException_when_given_title_alreadyExist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_=>_.Categories.Add(category));
            AddCategoryDto dto = CategoryFactory.CreateAddCategoryDto("لبنیات");

            Action expected= () => _sut.Add(dto);

            expected.Should().ThrowExactly<TitleAlreadyExistException>();
        }

        [Fact]
        public void Update_update_category_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            _sut.Update(category.Id, dto);

            _dataContext.Categories.Should()
                .Contain(x => x.Title == dto.Title);
        }

        [Fact]
        public void Update_throw_TitleAlreadyExistException_when_given_title_alreadyExist()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var secondCategory = CategoryFactory.CreateCategory("خشکبار");
            _dataContext.Manipulate(_ => _.Categories.Add(secondCategory));
            var dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            Action expected = () => _sut.Update(category.Id, dto);

            expected.Should().ThrowExactly<TitleAlreadyExistException>();
        }

        [Theory]
        [InlineData(500)]
        public void Update_throw_CategoryWithGivenIdDoesNotExist_when_given_id_does_not_exist(int fakeCategoryId)
        {
            var dto = CategoryFactory.CreateUpdateCategoryDto("خشکبار");

            Action expected = () => _sut.Update(fakeCategoryId, dto);

            expected.Should().ThrowExactly<CategoryWithGivenIdDoesNotExist>();
        }

        [Fact]
        public void GetAll_return_all_categories_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            var expected=_sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_=>_.Title==category.Title);
        }

        [Fact]
        public void Delete_delete_category_properly()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            _sut.Delete(category.Id);

            _dataContext.Categories.Should()
                .NotContain(_=>_.Title==category.Title);
        }

        [Theory]
        [InlineData(500)]
        public void Delete_throw_CategoryWithGivenIdDoesNotExist_when_given_id_does_not_exist(int fakeCategoryId)
        {
            Action expected = () => _sut.Delete(fakeCategoryId);

            expected.Should().ThrowExactly<CategoryWithGivenIdDoesNotExist>();
        }

        [Fact]
        public void Delete_throw_GoodsExistInCategoryException_when_goods_exist_in_category()
        {
            var category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var goods = GoodsFactory.CreateGoodsWithCategory(category.Id);
            _dataContext.Manipulate(_ => _.Goodss.Add(goods));

            Action expected = () => _sut.Delete(category.Id);

            expected.Should().ThrowExactly<GoodsExistInCategoryException>();
        }

    }
}
