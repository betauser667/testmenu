using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationCore.Models
{
    public class Dish : BaseModel
    {
        [Column(Order = 3)]
        public string Description { get; set; }

        [MaxLength(255)]
        [Column(Order = 5)]
        public string Image { get; set; }

        [Required]
        [Column(Order = 4)]
        public decimal Price { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
