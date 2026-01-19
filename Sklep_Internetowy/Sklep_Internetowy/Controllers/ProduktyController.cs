using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sklep_Internetowy.Models;

namespace Sklep_Internetowy.Controllers
{
	
	public class ProduktyController : Controller
    {
        private readonly SklepDbContext _context;

        public ProduktyController(SklepDbContext context)
        {
            _context = context;
        }

		// GET: Produkty
		public async Task<IActionResult> Index(int? kategoriaId)
		{
			var produktyQuery = _context.Produkties.Include(p => p.Kategoria).AsQueryable();

			if (kategoriaId != null)
			{
				produktyQuery = produktyQuery.Where(p => p.KategoriaId == kategoriaId);
			}

			return View(await produktyQuery.ToListAsync());
		}

		// GET: Produkty/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkty = await _context.Produkties
                .Include(p => p.Kategoria)
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkty == null)
            {
                return NotFound();
            }

            return View(produkty);
        }

		// GET: Produkty/Create
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
        {
            ViewData["KategoriaId"] = new SelectList(_context.Kategories, "KategoriaId", "Nazwa");
            return View();
        }

        // POST: Produkty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produkty produkty)
        {
			ModelState.Remove("Kategoria");

			if (ModelState.IsValid)
            {
                _context.Add(produkty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			ViewData["KategoriaId"] = new SelectList(_context.Kategories, "KategoriaId", "Nazwa", produkty.KategoriaId);
			return View(produkty);
        }

		// GET: Produkty/Edit/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkty = await _context.Produkties.FindAsync(id);
            if (produkty == null)
            {
                return NotFound();
            }
            ViewData["KategoriaId"] = new SelectList(_context.Kategories, "KategoriaId", "Nazwa", produkty.KategoriaId);
            return View(produkty);
        }

        // POST: Produkty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produkty produkty)
        {
			ModelState.Remove("Kategoria");

			if (id != produkty.ProduktId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produkty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduktyExists(produkty.ProduktId))
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
            ViewData["KategoriaId"] = new SelectList(_context.Kategories, "KategoriaId", "KategoriaId", produkty.KategoriaId);
            return View(produkty);
        }

		// GET: Produkty/Delete/5
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produkty = await _context.Produkties
                .Include(p => p.Kategoria)
                .FirstOrDefaultAsync(m => m.ProduktId == id);
            if (produkty == null)
            {
                return NotFound();
            }

            return View(produkty);
        }

        // POST: Produkty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkty = await _context.Produkties.FindAsync(id);
            if (produkty != null)
            {
                _context.Produkties.Remove(produkty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduktyExists(int id)
        {
            return _context.Produkties.Any(e => e.ProduktId == id);
        }
    }
}
