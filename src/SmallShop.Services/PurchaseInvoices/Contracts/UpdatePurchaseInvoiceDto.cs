using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.PurchaseInvoices.Contracts
{
    public class UpdatePurchaseInvoiceDto
    {
        
        public DateTime Date { get; set; }
        
        public int Count { get; set; }
        
        public int Price { get; set; }
        
        public int GoodsId { get; set; }
        
        public string SellerName { get; set; }
    }
}
