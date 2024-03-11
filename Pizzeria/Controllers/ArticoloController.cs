using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.data;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    public class ArticoloController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticoloController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Articolo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articoli.ToListAsync());
        }

        // GET: Articolo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articolo = await _context.Articoli
                .FirstOrDefaultAsync(m => m.IdArticolo == id);
            if (articolo == null)
            {
                return NotFound();
            }

            return View(articolo);
        }

        // GET: Articolo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articolo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArticolo,Nome,Immagine,Prezzo,TempoConsegna,Ingredienti")] Articolo articolo)
        {
            ModelState.Remove("DettagliOrdini");
            if (ModelState.IsValid)
            {
                _context.Add(articolo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articolo);
        }

        // GET: Articolo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articolo = await _context.Articoli.FindAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            return View(articolo);
        }

        // POST: Articolo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticolo,Nome,Immagine,Prezzo,TempoConsegna,Ingredienti")] Articolo articolo)
        {

            if (id != articolo.IdArticolo)
            {
                return NotFound();
            }
            ModelState.Remove("DettagliOrdini");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articolo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticoloExists(articolo.IdArticolo))
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
            return View(articolo);
        }

        // GET: Articolo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articolo = await _context.Articoli
                .FirstOrDefaultAsync(m => m.IdArticolo == id);
            if (articolo == null)
            {
                return NotFound();
            }

            return View(articolo);
        }

        // POST: Articolo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articolo = await _context.Articoli.FindAsync(id);
            if (articolo != null)
            {
                _context.Articoli.Remove(articolo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticoloExists(int id)
        {
            return _context.Articoli.Any(e => e.IdArticolo == id);
        }
    }
}
