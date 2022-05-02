using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Entities
{
    public class Goods
    {
        public Goods()
        {
            SaleInvoices = new HashSet<SaleInvoice>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
        }
        public int GoodsCode { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int GoodsInventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
        public int SellCount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public HashSet<SaleInvoice> SaleInvoices { get; set; }
        public HashSet<PurchaseInvoice> PurchaseInvoices { get; set; }
    }
}
