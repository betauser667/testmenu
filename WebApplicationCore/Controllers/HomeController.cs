using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorLight;
using WebApplicationCore.Core;
using WebApplicationCore.Data;
using WebApplicationCore.Models;

namespace WebApplicationCore.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _dbContext;
        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Preview(string id = "2")
        {
            try
            {



                // Create engine that uses entityFramework to fetch templates from db
                // You can create project that uses your IRepository<T>
                var project = new EntityFrameworkRazorLightProject(_dbContext);
                var engine = new RazorLightEngineBuilder().UseProject(project).Build();


                // As our key in database is integer, but engine takes string as a key - pass integer ID as a string
                string templateKey = id;
                var model = new TestViewModel() { Name = "Johny", Age = 22, Id = 101, XId = id, List = new List<string>() { "xxx", "zzz", "hhh" } };
                var result = engine.CompileRenderAsync(templateKey, model).Result;

                return Content(result, "text/html", System.Text.Encoding.UTF8);// View();
            }
            catch (Exception ex)
            {

                ViewData["Message"] = ex.Message + "\r\n" + ex.StackTrace;
                return View();
            }
            //return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult GetCurrentTimeComponent()
        {
            return ViewComponent("ViewComponentCurrentTime");
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


        public async Task<IActionResult> GetDishes(int count = 10)
        {
            var dishes = await _dbContext.Dishes.ToListAsync();
            //switch (type)
            //{
            //    //case "products": return new JsonResult(new { Type = type, table = new List<Product> { new Product() { Id = 11, Name = "Prod1", Price = 23.41M }, new Product() { Id = 12, Name = "Prod2", Price = 13.41M }, } });
            //    //case "dishes": return new JsonResult(new { Type = type, table = new List<Dish> { new Dish() { Id = 2, Name = "Dish one", Category = new Category() { Id = 10, Name = "Cat 1" }, Products = new List<Product>() { new Product() { Id = 11, Name = "Prod1", Price = 23.41M }, new Product() { Id = 12, Name = "Prod2", Price = 13.41M }, } } } });
            //    //case "categories": return new JsonResult(new { Type = type, table = new List<Category> { new Category() { Id = 10, Name = "Cat 10" }, new Category() { Id = 13, Name = "Cat 13" }, } });
            //}
            return new JsonResult(dishes);
        }
    }


}