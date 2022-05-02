using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Entities
{
    public class Invoice
    {
        public int InvoiceNum { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int GoodsId { get; set; }
        public Goods Goods { get; set; }
    }
}
