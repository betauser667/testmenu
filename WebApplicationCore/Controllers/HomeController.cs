using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using WebApplicationCore.Models;

namespace WebApplicationCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About(string id = null)
        {
            var config = new TemplateServiceConfiguration();
            // .. configure your instance

            var service = RazorEngineService.Create(config);
            config.Language = Language.CSharp; // VB.NET as template language.
            config.EncodedStringFactory = new RawStringFactory(); // Raw string encoding.
            config.EncodedStringFactory = new HtmlEncodedStringFactory(); // Html encoding.
            config.CachingProvider = new DefaultCachingProvider(t => { });
            Engine.Razor = service;

            string template =
            @"<!doctype html>
<html>
      <head>
        <title>Hello @Model.Name</title>
        <script src='https://code.jquery.com/jquery-latest.min.js'></script>
      </head>
      <body>        
<div>        <title>Hello @Model.Name !!!</title>
<div>If you want to use the static Engine class with this new configuration:</div>

<p>" + id + @"</p>
<p>@Model.id</p>
@foreach(var x in @Model.list) {
<li>@x</li>
}
<div id=""myComponentContainer"" name=""myComponentContainer"">xxx</div>
<script>
            var container = $('#myComponentContainer');
var b = true;
var refreshComponent = function() {
    $.get('/Home/MyViewComponent', function(data) { console.log(data); 
//if(b) { container.hide(); } else { container.show(); }
container.html(data); 
//b = !b;
});
            };
$(function() { window.setInterval(refreshComponent, 2000); });


$.get('/Home/Items?type=dishes&count=12', function(data) { console.log(data); });


</script>

</div>
      </body>
    </html>";

            var result = Engine.Razor.RunCompile(template, "templateKey", null, new { Name = "World", id = id, list = new List<string>() { "aaa", "bbb", "xxx" } });

            return Content(result, "text/html", System.Text.Encoding.UTF8);// View();
            //return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult MyViewComponent()
        {
            return ViewComponent("MyViewComponent");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Items(string type, int count = 10)
        {
            switch (type)
            {
                case "products": return new JsonResult(new { Type = type, table = new List<Product> { new Product() { Id = 11, Name = "Prod1", Price = 23.41M }, new Product() { Id = 12, Name = "Prod2", Price = 13.41M }, } });
                case "dishes": return new JsonResult(new { Type = type, table = new List<Dish> { new Dish() { Id = 2, Name = "Dish one", Category = new Category() { Id = 10, Name = "Cat 1" }, Products = new List<Product>() { new Product() { Id = 11, Name = "Prod1", Price = 23.41M }, new Product() { Id = 12, Name = "Prod2", Price = 13.41M }, } } } });
                case "categories": return new JsonResult(new { Type = type, table = new List<Category> { new Category() { Id = 10, Name = "Cat 10" }, new Category() { Id = 13, Name = "Cat 13" }, } });
            }
            return new JsonResult(new { });
        }
    }

    [ViewComponent(Name = "MyViewComponent")]
    public class MyViewComponent123 : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var time = DateTime.Now.ToString("h:mm:ss");
            return Content($"The current time is {time}");
        }
    }

    public class baseobj
    {
        public int Id;
        public string Name;
    }

    public class Product : baseobj
    {
        public decimal Price;
    }

    public class Category : baseobj
    {
        public Category Parent;
    }

    public class Dish : baseobj
    {
        public Category Category;
        public List<Product> Products;
    }
}