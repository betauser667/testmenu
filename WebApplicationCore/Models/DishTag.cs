namespace WebApplicationCore.Models
{
    public class DishTag
    {
        public virtual Dish Dish { get; set; }
        public int DishId { get; set; }

        public virtual Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
