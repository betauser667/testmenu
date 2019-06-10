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

                dbContext.Tags.Add(new Tag() { Name = "Tag1", CreatedDate = DateTime.Now });
                dbContext.Tags.Add(new Tag() { Name = "Tag2", CreatedDate = DateTime.Now });
                dbContext.Tags.Add(new Tag() { Name = "Tag3", CreatedDate = DateTime.Now });
                dbContext.SaveChanges();

                dbContext.Dishes.Add(new Dish() { Name = "Dish12", CreatedDate = DateTime.Now, Price = 100, Description = "Description1" });
                dbContext.Dishes.Add(new Dish() { Name = "Dish23", CreatedDate = DateTime.Now, Price = 120, Description = "Description2" });
                dbContext.Dishes.Add(new Dish() { Name = "Dish13", CreatedDate = DateTime.Now, Price = 150, Description = "Description3" });
                dbContext.Dishes.Add(new Dish() { Name = "Dish123", CreatedDate = DateTime.Now, Price = 250, Description = "Description4" });
                dbContext.SaveChanges();

                var dish = dbContext.Dishes.Find(1);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(1) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(2) });

                dish = dbContext.Dishes.Find(2);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(2) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(3) });

                dish = dbContext.Dishes.Find(3);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(1) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(3) });

                dish = dbContext.Dishes.Find(4);
                dish.Tags = new System.Collections.Generic.List<DishTag>();
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(1) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(2) });
                dish.Tags.Add(new DishTag() { Dish = dish, Tag = dbContext.Tags.Find(3) });
                dbContext.SaveChanges();
            }




        }
    }
}
