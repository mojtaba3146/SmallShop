using System.ComponentModel.DataAnnotations;

namespace SmallShop.Services.Goodss.Contracts
{
    public class UpdateGoodsDto
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
