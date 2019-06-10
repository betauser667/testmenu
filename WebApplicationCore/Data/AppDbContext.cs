using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationCore.Models;

namespace WebApplicationCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<TemplateEntity> Templates { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishTag>().HasKey(t => new { t.DishId, t.TagId });
        }

        //public static IEnumerable<IMutableEntityType> EntityTypes(this ModelBuilder builder)
        //{
        //    return builder.Model.GetEntityTypes();
        //}

        //public static IEnumerable<IMutableProperty> Properties(this ModelBuilder builder)
        //{
        //    return builder.EntityTypes().SelectMany(entityType => entityType.GetProperties());
        //}

        //public static IEnumerable<IMutableProperty> Properties<T>(this ModelBuilder builder)
        //{
        //    return 
        //}

        //public static void Configure(this IEnumerable<IMutableEntityType> entityTypes, Action<IMutableEntityType> convention)
        //{
        //    foreach (var entityType in entityTypes)
        //    {
        //        convention(entityType);
        //    }
        //}

        //public static void Configure(this IEnumerable<IMutableProperty> propertyTypes, Action<IMutableProperty> convention)
        //{
        //    foreach (var propertyType in propertyTypes)
        //    {
        //        convention(propertyType);
        //    }
        //}
    }
}
