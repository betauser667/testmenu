using System.Collections.Generic;

namespace WebApplicationCore.Models
{
    public class Tag : BaseModel
    {
        public virtual List<DishTag> Dishes { get; set; }
    }
}
