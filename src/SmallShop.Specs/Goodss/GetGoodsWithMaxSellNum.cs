﻿using FluentAssertions;
using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Test;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.Persistence.EF.Goodss;
using SmallShop.Services.Categories.Contracts;
using SmallShop.Services.Goodss;
using SmallShop.Services.Goodss.Contracts;
using SmallShop.Specs.Infrastructure;
using SmallShop.Test.Tools.Categories;
using SmallShop.Test.Tools.Goodss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static SmallShop.Specs.BDDHelper;

namespace SmallShop.Specs.Goodss
{
    [Scenario("مشاهده کالای پر فروش")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "  مشاهده کالای پر فروش ",
        InOrderTo = "پرفروش تریت کالا را مشاهده کنم"
    )]
    public class GetGoodsWithMaxSellNum : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly GoodsService _sut;
        private readonly GoodsRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _categoryRepository;
        private Category _category;
        private Goods _goods,_goodsTwo;
        public GetGoodsWithMaxSellNum(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFGoodsRepository(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new GoodsAppService(_repository, _unitOfWork, _categoryRepository);
        }

        [Given("کالایی به نام 'ماست رامک' و قیمت '500' و کد کالای '10' و حداقل موجودی '20' و حداکثر موجودی '40' و تعداد فروش '5' در دسته 'لبنیات' وجود دارد")]
        public void Given()
        {
            CreateFirstGoods();
        }

        [And("کالایی به نام 'ماست میهن' و قیمت '500' و کد کالای '20' و حداقل موجودی '20' و حداکثر موجودی '40' و تعداد فروش '11' در دسته 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            CreateSecondGoods();
        }

        [When("درخواست مشاهده کالایی با فروش بیشتر را دارم")]
        public void When()
        {
            _sut.GetBestSellerGoods();
        }

        [Then("کالایی به نام 'ماست میهن' و کد کالای '20'  در دسته 'لبنیات' نشان داده می شود")]
        public void Then()
        {
            var expected = _dataContext.Goodss.ToList();

            expected.Should().Contain(_ => _.Name == _goodsTwo.Name);
            expected.Should().Contain(_ => _.GoodsCode == _goodsTwo.GoodsCode);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
        }

        private void CreateFirstGoods()
        {
            _category = CategoryFactory.CreateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
            _goods = new GoodsBuilder(_category.Id).WithSellCount(5)
                .WithGoodsInventory(21).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goods));
        }

        private void CreateSecondGoods()
        {
            _goodsTwo = new GoodsBuilder(_category.Id).WithName("ماست میهن")
                            .WithGoodsCode(20).WithGoodsInventory(22)
                            .WithSellCount(11).Build();
            _dataContext.Manipulate(_ => _.Goodss.Add(_goodsTwo));
        }
    }
}
