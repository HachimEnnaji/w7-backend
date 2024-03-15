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
            var carrelloSession = HttpContext.Session.GetString("carrelloList");

            if (string.IsNullOrEmpty(carrelloSession))
            {
                TempData["error"] = "Carrello vuoto";
                return RedirectToAction("MostraCarrello", "Articolo");
            }
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Ordines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrdine,IdUtente,Indirizzo,IsConsegnato,Note,DataOrdine,PrezzoTotale")] Ordine ordine)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ordine.IdUtente = Convert.ToInt32(userId);
            ordine.DataOrdine = DateTime.Now;

            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();


            var carrelloSession = HttpContext.Session.GetString("carrelloList");

            if (!string.IsNullOrEmpty(carrelloSession))
            {
                var carrello = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);
                double prezzoTotale = 0;
                foreach (var item in carrello)
                {

                    var dettagliOrdine = new DettagliOrdine
                    {
                        IdOrdine = ordine.IdOrdine,
                        IdArticolo = item.Articolo.IdArticolo,
                        Quantita = item.Quantita,
                        PrezzoUnitario = item.Articolo.Prezzo
                    };
                    _context.DettagliOrdini.Add(dettagliOrdine);
                    prezzoTotale += item.Quantita * item.Articolo.Prezzo;


                }
                ordine.PrezzoTotale = prezzoTotale;
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("carrelloList");
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

        public async Task<IActionResult> InConsegna()
        {
            var inConsegna = await _context.Ordini.Where(o => o.IsConsegnato == false).ToListAsync();
            return View(inConsegna);
        }
        [HttpPost]
        public async Task<IActionResult> Consegnato(int id)
        {
            try
            {
                var ordine = await _context.Ordini.FindAsync(id);

                if (ordine == null)
                {
                    return StatusCode(404, "Article not found");
                }
                ordine.IsConsegnato = true;
                _context.Update(ordine);
                await _context.SaveChangesAsync();
                return StatusCode(200, "Article updated");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
