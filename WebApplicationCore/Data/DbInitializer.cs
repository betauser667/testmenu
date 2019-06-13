using System;
using System.Linq;
using WebApplicationCore.Models;

namespace WebApplicationCore.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext dbContext)
        {
            if (!dbContext.Templates.Any())
            {
                dbContext.Templates.Add(new TemplateEntity()
                {
                    Name = "DateTime Tempalte",
                    Content = @"<html><head>
<title>Hello @Model.Name !!!</title>
<script src='https://code.jquery.com/jquery-latest.min.js'></script>
</head>
<body>
<div>
<title>Hello @Model.Name !!!</title>
</div>
<h1>@System.Diagnostics.Process.GetCurrentProcess().ProcessName</h1>
<h2>Current DateTime: @System.DateTime.Now UTC: @System.DateTime.UtcNow</h2>
<div>
Request time from backend: <div id=""DateTimeContainer"" name=""DateTimeContainer""></div>
<div>
<script>
var refreshComponent = function() { $.get('/Home/GetCurrentTimeComponent', function(data) { $('#DateTimeContainer').html(data); }); };    
$(function(){ window.setInterval(refreshComponent, 2000); });
</script>
</body></html>"
                });

                dbContext.Templates.Add(new TemplateEntity()
                {
                    Name = "Dishes Tempalte",
                    Content = @"<!doctype html>
<html>
<head>
<title>Hello @Model.Name</title>  
<script src='https://code.jquery.com/jquery-latest.min.js'></script>
</head>
<body>
<div>
<title>Hello @Model.Name !!!</title>
<div>If you want to use the static Engine class with this new configuration:</div>
<p> @Model.XId </p>
<p> @Model.Id </p>
@foreach(var x in Model.List) {<li> @x </li>}    
<div id = ""dishesContainer"" name = ""dishesContainer"" ></div>
<script>
$.get('/Home/GetDishes?count=12', function(data) {
console.log(data);
var datastring = '';
for (var i = 0; i < data.length; i++)
{
    d = data[i];
    datastring += ""<div id='"" + d.id + ""'><h2>"" + d.name + ""</h2><image src='"" + d.image + ""'/><h3>Price:"" + d.price + ""</h3><p>"" + d.description + ""</p><p>Add date:"" + d.createdDate + ""</p></div>"";
}
 $('#dishesContainer').html(datastring);
});    
</script>
</div>
</body>
</html>"
                });
                dbContext.SaveChanges();
                
                dbContext.Tags.Add(new Tag() { Name = "Meet", CreatedDate = DateTime.Now });
                dbContext.Tags.Add(new Tag() { Name = "Breat", CreatedDate = DateTime.Now });
                dbContext.Tags.Add(new Tag() { Name = "Salat", CreatedDate = DateTime.Now });
                dbContext.SaveChanges();

                dbContext.Ingredients.Add(new Ingredient() { Name = "Onion", CreatedDate = DateTime.Now, Calories = 200, Image = "https://fermercenter.com/image/data/1Delfis/galant1.jpg" });
                dbContext.Ingredients.Add(new Ingredient() { Name = "Pork", CreatedDate = DateTime.Now, Calories= 600, Image= "https://st.depositphotos.com/2377011/3382/i/450/depositphotos_33822771-stock-photo-pork-neck.jpg" });
                dbContext.Ingredients.Add(new Ingredient() { Name = "Salat", CreatedDate = DateTime.Now, Calories = 2, Image = "https://images.ichkoche.at/data/image/variations/620x434/8/gruener-salat-img-70025.jpg" });
                dbContext.Ingredients.Add(new Ingredient() { Name = "Bulka", CreatedDate = DateTime.Now, Calories = 400, Image = "https://static2.tgstat.com/public/images/channels/_0/60/60aa66c3f7b121c983aa827a020b81d7.jpg" });
                dbContext.Ingredients.Add(new Ingredient() { Name = "Beef", CreatedDate = DateTime.Now, Calories = 500, Image = "http://eda.36on.ru/uploads/article/large_picture/173/govyadina.jpg" });
                dbContext.SaveChanges();


                dbContext.Dishes.Add(new Dish() { Name = "Burger pork", CreatedDate = DateTime.Now, Price = 100, Description = "Description1", Image = "https://www.tasteofhome.com/wp-content/uploads/2018/01/Grilled-Pork-Burgers_EXPS_SDJJ18_43741_D02_08_4b-696x696.jpg" });
                dbContext.Dishes.Add(new Dish() { Name = "Beef burger", CreatedDate = DateTime.Now, Price = 120, Description = "Description2", Image = "https://pizza-pizza.co.uk/wp-content/uploads/2015/12/beef-burger1-1.jpg" });
                dbContext.Dishes.Add(new Dish() { Name = "Salat", CreatedDate = DateTime.Now, Price = 150, Description = "Description3", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2mQi1yXZvTUBXz0htQXekS7iceD9cAB6jvp1DyhHHjcNdybsOZQ" });
                dbContext.Dishes.Add(new Dish() { Name = "Green burger", CreatedDate = DateTime.Now, Price = 250, Description = "Description4", Image = "https://cms.splendidtable.org/sites/default/files/styles/w2000/public/veggie-burgers.jpg" });
                dbContext.SaveChanges();

                var dish = dbContext.Dishes.Find(1);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(1) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(2) });
                dish.Ingredients = new System.Collections.Generic.List<DishIngredient>();
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(1), Volume = "30 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(2), Volume = "180 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(3), Volume = "50 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(4), Volume = "200 rg" });
                dbContext.SaveChanges();

                dish = dbContext.Dishes.Find(2);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(1) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(2) });
                dish.Ingredients = new System.Collections.Generic.List<DishIngredient>();
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(3), Volume = "50 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(4), Volume = "250 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(5), Volume = "300 rg" });
                dbContext.SaveChanges();

                dish = dbContext.Dishes.Find(3);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(3) });
                dish.Ingredients = new System.Collections.Generic.List<DishIngredient>();
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(1), Volume = "50 rg" });                
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(3), Volume = "200 rg" });
                dbContext.SaveChanges();

                dish = dbContext.Dishes.Find(4);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(2) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(3) });
                dish.Ingredients = new System.Collections.Generic.List<DishIngredient>();
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(1), Volume = "50 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(3), Volume = "200 rg" });
                dish.Ingredients.Add(new DishIngredient() { Dish = dish, Ingredient = dbContext.Ingredients.Find(4), Volume = "150 rg" });
                dbContext.SaveChanges();
            }




        }
    }
}
