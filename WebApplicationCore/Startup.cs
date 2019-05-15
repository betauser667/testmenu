using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApplicationCore.Data;
using WebApplicationCore.Models;

namespace WebApplicationCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            AddTransients(services);

            //services.AddDbContextPool<AppDbContext>(options => options.UseMySql(Configuration.GetConnectionString("REContext"),
            //    mysqlOptions =>
            //    {
            //        mysqlOptions.ServerVersion(new Version(5, 6), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
            //    }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Main", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "Main" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/swagger.json", "My API");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void AddTransients(IServiceCollection services)
        {
            services.AddTransient(typeof(AppDbContext), (AppDbContext) =>
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
              .UseInMemoryDatabase(databaseName: "TestDatabase")
              .Options;

                // Create and fill database with test data
                var dbContext = new AppDbContext(options);
                FillDatabase(dbContext);
                return dbContext;
            });
            services.AddTransient(typeof(IConfiguration), x => Configuration);

            //var redisConnectionProperties = new ConfigurationOptions()
            //{
            //    SyncTimeout = 1000,
            //    AsyncTimeout = 1000,
            //    EndPoints = { new DnsEndPoint(Configuration["RedisConnectionStringHost"], Int32.Parse(Configuration["RedisConnectionStringPort"])) },
            //    AbortOnConnectFail = false // this prevents that error
            //};
            //services.AddSingleton<IConnectionMultiplexer>(ctx => ConnectionMultiplexer.Connect(redisConnectionProperties));
            //services.AddTransient<ICacheService, RedisService>(r => new RedisService(r.GetService<IConnectionMultiplexer>(), Configuration["EnvironmentName"]));
            //services.AddTransient(typeof(RuleEntriesService), typeof(RuleEntriesService));
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

        //class MyIReferenceResolver : IReferenceResolver
        //{
        //    public string FindLoaded(IEnumerable<string> refs, string find)
        //    {
        //        return refs.First(r => r.EndsWith(System.IO.Path.DirectorySeparatorChar + find));
        //    }

        //    public IEnumerable<CompilerReference> GetReferences(TypeContext context, IEnumerable<CompilerReference> includeAssemblies = null)
        //    {
        //        // TypeContext gives you some context for the compilation (which templates, which namespaces and types)

        //        // You must make sure to include all libraries that are required!
        //        // Mono compiler does add more standard references than csc! 
        //        // If you want mono compatibility include ALL references here, including mscorlib!
        //        // If you include mscorlib here the compiler is called with /nostdlib.
        //        IEnumerable<string> loadedAssemblies = (new UseCurrentAssembliesReferenceResolver())
        //            .GetReferences(context, includeAssemblies)
        //            .Select(r => r.GetFile())
        //            .ToArray();

        //        yield return CompilerReference.From(FindLoaded(loadedAssemblies, "mscorlib.dll"));
        //        yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.dll"));
        //        yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Core.dll"));
        //        yield return CompilerReference.From(typeof(MyIReferenceResolver).Assembly); // Assembly

        //        // There are several ways to load an assembly:
        //        //yield return CompilerReference.From("Path-to-my-custom-assembly"); // file path (string)
        //        //byte[] assemblyInByteArray = --- Load your assembly ---;
        //        //yield return CompilerReference.From(assemblyInByteArray); // byte array (roslyn only)
        //        //string assemblyFile = --- Get the path to the assembly ---;
        //        //yield return CompilerReference.From(File.OpenRead(assemblyFile)); // stream (roslyn only)
        //    }
        //}

    }
}
