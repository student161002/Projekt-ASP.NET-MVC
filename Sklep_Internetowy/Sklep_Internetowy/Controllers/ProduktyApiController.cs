using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sklep_Internetowy.Models;

namespace Sklep_Internetowy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduktyApiController : ControllerBase
    {
        private readonly SklepDbContext _context;

        public ProduktyApiController(SklepDbContext context)
        {
            _context = context;
        }

        // GET: api/ProduktyApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkty>>> GetProdukties()
        {
            return await _context.Produkties.ToListAsync();
        }

        // GET: api/ProduktyApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produkty>> GetProdukty(int id)
        {
            var produkty = await _context.Produkties.FindAsync(id);

            if (produkty == null)
            {
                return NotFound();
            }

            return produkty;
        }

        // PUT: api/ProduktyApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdukty(int id, Produkty produkty)
        {
            if (id != produkty.ProduktId)
            {
                return BadRequest();
            }

            _context.Entry(produkty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduktyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProduktyApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produkty>> PostProdukty(Produkty produkty)
        {
            _context.Produkties.Add(produkty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdukty", new { id = produkty.ProduktId }, produkty);
        }

        // DELETE: api/ProduktyApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdukty(int id)
        {
            var produkty = await _context.Produkties.FindAsync(id);
            if (produkty == null)
            {
                return NotFound();
            }

            _context.Produkties.Remove(produkty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduktyExists(int id)
        {
            return _context.Produkties.Any(e => e.ProduktId == id);
        }
    }
}
