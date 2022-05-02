using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallShop.Services.Categories.Contracts
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Title { get; set; }
    }
}
