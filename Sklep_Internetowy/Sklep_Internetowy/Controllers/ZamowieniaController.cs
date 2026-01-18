using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sklep_Internetowy.Models;

namespace Sklep_Internetowy.Controllers
{
    public class ZamowieniaController : Controller
    {
        private readonly SklepDbContext _context;

        public ZamowieniaController(SklepDbContext context)
        {
            _context = context;
        }

        // GET: Zamowienia
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zamowienia.ToListAsync());
        }

        // GET: Zamowienia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienium = await _context.Zamowienia
                .FirstOrDefaultAsync(m => m.ZamowienieId == id);
            if (zamowienium == null)
            {
                return NotFound();
            }

            return View(zamowienium);
        }

		// GET: Zamowienia/Create
		// GET: Zamowienia/Create?produktId=5
		public IActionResult Create(int? produktId)
		{
			if (produktId == null)
			{
				return NotFound();
			}

			var produkt = _context.Produkties.Find(produktId);

			if (produkt == null)
			{
				return NotFound();
			}

			ViewBag.ProduktNazwa = produkt.Nazwa;
			ViewBag.ProduktCena = produkt.Cena;
			ViewBag.ProduktId = produkt.ProduktId;

			return View();
		}
		// GET: Zamowienia/Potwierdzenie/5
		public IActionResult Potwierdzenie(int id)
		{
			return View(id); // Przekazujemy ID zamówienia do widoku
		}

		// POST: Zamowienia/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		// POST: Zamowienia/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(int ProduktId, [Bind("ZamowienieId,DataZamowienia,AdresWysylki,ImieNazwiskoKlienta")] Zamowienium zamowienie)
		{
			zamowienie.DataZamowienia = DateTime.Now;

			ModelState.Remove("PozycjeZamowienia");

			if (ModelState.IsValid)
			{
				_context.Add(zamowienie);
				await _context.SaveChangesAsync(); 

				var produkt = await _context.Produkties.FindAsync(ProduktId);

				if (produkt != null)
				{
					
					var pozycja = new PozycjeZamowienium
					{
						ZamowienieId = zamowienie.ZamowienieId,
						ProduktId = ProduktId,
						Ilosc = 1, 
						CenaJednostkowa = produkt.Cena
					};

					_context.Add(pozycja);

					zamowienie.WartoscCalkowita = produkt.Cena;
					_context.Update(zamowienie);

					await _context.SaveChangesAsync();
				}

				return RedirectToAction("Potwierdzenie", new { id = zamowienie.ZamowienieId });
			}

			return View(zamowienie);
		}

		// GET: Zamowienia/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienium = await _context.Zamowienia.FindAsync(id);
            if (zamowienium == null)
            {
                return NotFound();
            }
            return View(zamowienium);
        }

        // POST: Zamowienia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZamowienieId,DataZamowienia,AdresWysylki,ImieNazwiskoKlienta,WartoscCalkowita")] Zamowienium zamowienium)
        {
            if (id != zamowienium.ZamowienieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zamowienium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowieniumExists(zamowienium.ZamowienieId))
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
            return View(zamowienium);
        }

        // GET: Zamowienia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienium = await _context.Zamowienia
                .FirstOrDefaultAsync(m => m.ZamowienieId == id);
            if (zamowienium == null)
            {
                return NotFound();
            }

            return View(zamowienium);
        }

        // POST: Zamowienia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienium = await _context.Zamowienia.FindAsync(id);
            if (zamowienium != null)
            {
                _context.Zamowienia.Remove(zamowienium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowieniumExists(int id)
        {
            return _context.Zamowienia.Any(e => e.ZamowienieId == id);
        }
    }
}
