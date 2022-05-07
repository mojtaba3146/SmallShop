using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.SaleInvoices.Contracts
{
    public class AddSaleInvoiceDto
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
        public string BuyerName { get; set; }
    }
}
