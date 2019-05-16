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
    public class TemplateEntitiesController : Controller
    {
        private readonly AppDbContext _context;

        public TemplateEntitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TemplateEntities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Templates.ToListAsync());
        }

        // GET: TemplateEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templateEntity = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (templateEntity == null)
            {
                return NotFound();
            }

            return View(templateEntity);
        }

        // GET: TemplateEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TemplateEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content")] TemplateEntity templateEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(templateEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(templateEntity);
        }

        // GET: TemplateEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templateEntity = await _context.Templates.FindAsync(id);
            if (templateEntity == null)
            {
                return NotFound();
            }
            return View(templateEntity);
        }

        // POST: TemplateEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Content")] TemplateEntity templateEntity)
        {
            if (id != templateEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(templateEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateEntityExists(templateEntity.Id))
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
            return View(templateEntity);
        }

        // GET: TemplateEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templateEntity = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (templateEntity == null)
            {
                return NotFound();
            }

            return View(templateEntity);
        }

        // POST: TemplateEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var templateEntity = await _context.Templates.FindAsync(id);
            _context.Templates.Remove(templateEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemplateEntityExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }
    }
}
