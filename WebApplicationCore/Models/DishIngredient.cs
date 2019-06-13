namespace WebApplicationCore.Models
{
    public class DishIngredient
    {
        public virtual Dish Dish { get; set; }
        public int DishId { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }

        public string Volume { get; set; }
    }
}
