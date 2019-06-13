using System.ComponentModel.DataAnnotations;

namespace WebApplicationCore.Models
{
    public class Ingredient : BaseModel
    {
        public int Calories { get; set; }

        [MaxLength(512)]
        public string Image { get; set; }
    }

}
