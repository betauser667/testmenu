using RazorLight.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationCore.Models
{
    public class baseobj
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Product : baseobj
    {
        public decimal Price { get; set; }
    }

    public class Category : baseobj
    {
        public Category Parent { get; set; }
    }

    public class Dish : baseobj
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
