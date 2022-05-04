using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Goodss.Contracts
{
    public class GetAllGoodsDto
    {
        [Required]
        public int GoodsCode { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int MinInventory { get; set; }
        [Required]
        public int MaxInventory { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
