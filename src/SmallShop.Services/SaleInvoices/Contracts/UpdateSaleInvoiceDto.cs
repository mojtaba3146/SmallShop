using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.SaleInvoices.Contracts
{
    public class UpdateSaleInvoiceDto
    {
        public DateTime Date { get; set; }

        public int Count { get; set; }

        public int Price { get; set; }

        public int GoodsId { get; set; }

        public string BuyerName { get; set; }
    }
}
