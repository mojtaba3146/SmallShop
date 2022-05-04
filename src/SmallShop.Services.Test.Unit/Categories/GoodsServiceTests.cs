using FluentAssertions;
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

namespace SmallShop.Services.Test.Unit.Categories
{
    public class GoodsServiceTests 
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly CategoryRepository _categoryRepository;

        public GoodsServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork,_categoryRepository);  
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
        [InlineData(40,20)]
        public void Add_throw_CategoryNotFoundException_when_category_with_given_id_not_exists(int fakeCategoryId,int categoryID)
        {
            var dto = GoodsFactory.CreateAddGoodsDto(categoryID);
            dto.CategoryId = fakeCategoryId;

            Action expected = () => _sut.Add(dto);
            expected.Should().ThrowExactly<CategoryNotFoundException>();
        }
    }
}
