using SmallShop.Entities;
using SmallShop.Services.PurchaseInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Test.Tools.PurchaseInvoices
{
    public static class PurchaseInvoiceFactory
    {
        public static AddPurchaseInvoiceDto CreateAddPurchaseInvoiceDto(int goodsCode)
        {
            return new AddPurchaseInvoiceDto
            {
                InvoiceNum = 1,
                Date = DateTime.Now.Date,
                GoodsId = goodsCode,
                SellerName = "sepehr",
                Count = 20,
                Price = 400
            };
        }

        public static PurchaseInvoice CreatePurchaseInvoice(int goodsCode)
        {
            return new PurchaseInvoice
            {
                Date = DateTime.Now.Date,
                InvoiceNum = 1,
                SellerName = "sepehr",
                Price = 400,
                Count = 20,
                GoodsId=goodsCode
            };
        }
    }
}
