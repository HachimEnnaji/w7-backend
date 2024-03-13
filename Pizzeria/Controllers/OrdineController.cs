using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pizzeria.data;
using Pizzeria.Models;
using System.Security.Claims;

namespace Pizzeria.Controllers
{
    public class OrdineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ordines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ordini.Include(o => o.Utente);
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ordines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordine = await _context.Ordini
                .Include(o => o.Utente)
                .FirstOrDefaultAsync(m => m.IdOrdine == id);
            if (ordine == null)
            {
                return NotFound();
            }

            return View(ordine);
        }

        // GET: Ordines/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Ordines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrdine,IdUtente,Indirizzo,IsConsegnato,Note")] Ordine ordine)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            System.Diagnostics.Debug.WriteLine(" SONO USER ID \t" + userId);
            System.Diagnostics.Debug.WriteLine(" SONO USER ID \t" + Convert.ToInt32(userId));

            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ordine.IdUtente = Convert.ToInt32(userId);

            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();


            var carrelloSession = HttpContext.Session.GetString("carrelloList");
            System.Diagnostics.Debug.WriteLine(" SONO carrello list \t" + carrelloSession);

            if (!string.IsNullOrEmpty(carrelloSession))
            {
                var carrello = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);
                foreach (var item in carrello)
                {
                    System.Diagnostics.Debug.WriteLine(" SONO carrello list riga 84 \t" + carrelloSession);

                    var dettagliOrdine = new DettagliOrdine
                    {
                        IdOrdine = ordine.IdOrdine,
                        IdArticolo = item.Articolo.IdArticolo,
                        Quantita = item.Quantita,
                        PrezzoUnitario = item.Articolo.Prezzo
                    };
                    _context.DettagliOrdini.Add(dettagliOrdine);
                }
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("CarrelloList");
                TempData["success"] = "Ordine effettuato con successo";
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Ordines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordine = await _context.Ordini.FindAsync(id);
            if (ordine == null)
            {
                return NotFound();
            }
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "IdUtente", "Username", ordine.IdUtente);
            return View(ordine);
        }

        // POST: Ordines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrdine,IdUtente,Indirizzo,IsConsegnato,Note")] Ordine ordine)
        {
            if (id != ordine.IdOrdine)
            {
                return NotFound();
            }
            ModelState.Remove("DettagliOrdini");
            ModelState.Remove("Utente");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdineExists(ordine.IdOrdine))
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
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "IdUtente", "Username", ordine.IdUtente);
            return View(ordine);
        }

        // GET: Ordines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordine = await _context.Ordini
                .Include(o => o.Utente)
                .FirstOrDefaultAsync(m => m.IdOrdine == id);
            if (ordine == null)
            {
                return NotFound();
            }

            return View(ordine);
        }

        // POST: Ordines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordine = await _context.Ordini.FindAsync(id);
            if (ordine != null)
            {
                _context.Ordini.Remove(ordine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdineExists(int id)
        {
            return _context.Ordini.Any(e => e.IdOrdine == id);
        }
    }
}
