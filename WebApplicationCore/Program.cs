using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorLight;
using RazorLight.Razor;

namespace WebApplicationCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public class TestViewModel
    {
        public string Name { get; set; }

        public int Age { get; set; }
        public int Id { get; set; }
        public string XId { get; set; }
        public List<string> List { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TemplateEntity> Templates { get; set; }
    }

    //This is simple POCO that represents your template that is stored in database
    public class TemplateEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class EntityFrameworkRazorProjectItem : RazorLightProjectItem
    {
        private string _content;

        public EntityFrameworkRazorProjectItem(string key, string content)
        {
            Key = key;
            _content = content;
        }

        public override string Key { get; }

        public override bool Exists => _content != null;

        public override Stream Read()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(_content));
        }
    }

    public class EntityFrameworkRazorLightProject : RazorLightProject
    {
        private readonly AppDbContext dbContext;

        public EntityFrameworkRazorLightProject()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDatabase")
              .Options;

            // Create and fill database with test data
            dbContext = new AppDbContext(options);
            FillDatabase(dbContext);
        }

        public EntityFrameworkRazorLightProject(AppDbContext context)
        {
            dbContext = context;
        }

        public override async Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        {
            // We expect id to be an integer, as in this sample we have ints as keys in database.
            // But you can use GUID, as an example and parse it here
            int templateId = int.Parse(templateKey);

            TemplateEntity template = await dbContext.Templates.FindAsync(templateId);

            var projectItem = new EntityFrameworkRazorProjectItem(templateKey, template?.Content);

            return projectItem;
        }

        public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync(string templateKey)
        {
            return Task.FromResult(Enumerable.Empty<RazorLightProjectItem>());
        }

        static void FillDatabase(AppDbContext dbContext)
        {
            dbContext.Templates.Add(new TemplateEntity()
            {
                Id = 1,
                Content = @"
                    <html>
                        @RenderBody()
                    </html>"
            });

            dbContext.Templates.Add(new TemplateEntity()
            {
                Id = 2,
                Content = @"<!doctype html>
<html>
      <head>
        <title>Hello @Model.Name</title>
        <script src='https://code.jquery.com/jquery-latest.min.js'></script>
      </head>
      <body>        
<div>        <title>Hello @Model.Name !!!</title>
<div>If you want to use the static Engine class with this new configuration:</div>
<p> @Model.XId </p>
<p> @Model.Id </p>
@foreach(var x in Model.List) {
<li> @x </li>
}
<div id=""myComponentContainer"" name=""myComponentContainer"">xxx</div>
<script>
            var container = $('#myComponentContainer');
var b = true;
var refreshComponent = function() {
    $.get('/Home/MyViewComponent', function(data) { console.log(data); 
container.html(data); 
});
            };
$(function() { window.setInterval(refreshComponent, 2000); });
$.get('/Home/Items?type=dishes&count=12', function(data) { console.log(data); });
</script>
</div>
      </body>
    </html>"
                //Content = @"
                //    @model Samples.EntityFrameworkProject.TestViewModel
                //    @{
                //        Layout = 1.ToString(); //This is an ID of your layout in database
                //     }
                //    <body> Hello, my name is @Model.Name and I am @Model.Age </body>"
            });
        }


    }
}
