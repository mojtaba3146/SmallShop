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
    }
}
