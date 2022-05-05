using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.PurchaseInvoices.Contracts
{
    public class AddPurchaseInvoiceDto
    {
        [Required]
        public int InvoiceNum { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int GoodsId { get; set; }
        [Required]
        public string SellerName { get; set; }
    }
}
