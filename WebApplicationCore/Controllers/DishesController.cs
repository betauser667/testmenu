using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationCore.Data;
using WebApplicationCore.Models;

namespace WebApplicationCore.Controllers
{
    public class DishesController : Controller
    {
        private readonly AppDbContext _context;

        public DishesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int? id)
        {
            var tags = _context.Tags.ToList();
            ViewData["AllTags"] = tags;
            var tl = await _context.Dishes.Include(d => d.Tags).ToListAsync();
            var tl1 = await _context.Dishes.Include(d => d.Tags).Where(d => d.Tags.Any(t => t.TagId == id)).ToListAsync();

            var list = await _context.Dishes.Include(d => d.Tags).Where(d => id == null || d.Tags.Any(t => t.TagId == id)).ToListAsync();
            return View(list);
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.Tags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            var tags = _context.Tags.ToList();
            ViewData["AllTags"] = tags;
            return View(new Dish() { CreatedDate = DateTime.Now, Tags = new List<DishTag>() });
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Image,Price,Id,Name,CreatedDate, TTag")] Dish dish, string selectedTags)
        {
            if (ModelState.IsValid)
            {
                if (selectedTags != null)
                {
                    var tagIds = selectedTags.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var listTags = _context.Tags.Where(t => tagIds.Any(it => it == t.Id.ToString())).ToList();
                    dish.Tags = listTags.Select(t => new DishTag() { Dish = dish, Tag = t }).ToList();
                }
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (selectedTags != null)
                {
                    var tagIds = selectedTags.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var listTags = _context.Tags.Where(t => tagIds.Any(it => it == t.Id.ToString())).ToList();
                    dish.Tags = listTags.Select(t => new DishTag() { Dish = dish, Tag = t }).ToList();
                }
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.Tags).FirstOrDefaultAsync(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            var tags = _context.Tags.ToList();
            ViewData["AllTags"] = tags;

            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Description,Image,Price,Id,Name,CreatedDate, TTag")] Dish dish, string selectedTags)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dishDb = await _context.Dishes.Include(d => d.Tags).FirstOrDefaultAsync(d => d.Id == id);

                    dishDb.Image = dish.Image;
                    dishDb.Name = dish.Name;
                    dishDb.Price = dish.Price;
                    dishDb.Products = dish.Products;
                    dishDb.Category = dish.Category;
                    dishDb.Description = dish.Description;

                    if (selectedTags != null)
                    {
                        // something changed
                        var tagIds = selectedTags?.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                        var listTags = _context.Tags.Where(t => tagIds.Any(it => it == t.Id.ToString())).ToList();
                        dishDb.Tags = listTags.Select(t => new DishTag() { Dish = dish, Tag = t }).ToList();
                    }
                    _context.Update(dishDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (selectedTags != null)
                {
                    var tagIds = selectedTags.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    var listTags = _context.Tags.Where(t => tagIds.Any(it => it == t.Id.ToString())).ToList();
                    dish.Tags = listTags.Select(t => new DishTag() { Dish = dish, Tag = t }).ToList();
                }
            }
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
    }
}
