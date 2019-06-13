using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationCore.Data;
using WebApplicationCore.Models;

namespace WebApplicationCore.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public class GetAllParametres
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public string[] Tags { get; set; } = new string[0];
            public string[] Ingredients { get; set; } = new string[0];
        }

        public DishesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<List<Dish>>> GetDishes([FromQuery]GetAllParametres parameters)
        {
            var query = _context.Dishes.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize)
                .Where(d => (!parameters.Tags.Any() || parameters.Tags.All(t => d.Tags.Any(dt => dt.TagId.ToString() == t)))
                    && (!parameters.Ingredients.Any() || parameters.Ingredients.All(t => d.Ingredients.Any(di => di.IngredientId.ToString() == t))))
                .Include(d => d.Category)
                .Include(d => d.Tags).ThenInclude(t => t.Tag)
                .Include(d => d.Ingredients).ThenInclude(t => t.Ingredient);

            var list = await query.ToListAsync();

            return list;
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.Tags).ThenInclude(t => t.Tag)
                .Include(d => d.Ingredients).ThenInclude(t => t.Ingredient)
                .Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }
    }
}
