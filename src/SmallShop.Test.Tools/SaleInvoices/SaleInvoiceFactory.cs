using SmallShop.Entities;
using SmallShop.Services.SaleInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Test.Tools.SaleInvoices
{
    public static class SaleInvoiceFactory
    {
        public static AddSaleInvoiceDto CreateAddSaleInvoiceDto(int goodsCode)
        {
            return new AddSaleInvoiceDto
            {
                BuyerName = "sepehr",
                InvoiceNum = 1,
                Count = 2,
                Date = DateTime.Now.Date,
                Price = 500,
                GoodsId = goodsCode
            };
        }

        public static SaleInvoice CreateSaleInvoice(int goodsCode)
        {
            return new SaleInvoice
            {
                BuyerName = "sepehr",
                InvoiceNum = 1,
                Count = 2,
                Date = DateTime.Now.Date,
                Price = 500,
                GoodsId = goodsCode
            };
        }

        public static UpdateSaleInvoiceDto CreateSaleInvoiceUpdateDto(int goodsCode)
        {
            return new UpdateSaleInvoiceDto
            {
                BuyerName = "Amin",
                Count = 2,
                Date = DateTime.Now.Date,
                Price = 500,
                GoodsId = goodsCode
            };
        }

    }
}
