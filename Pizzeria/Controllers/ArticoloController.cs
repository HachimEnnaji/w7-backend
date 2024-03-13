using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ArticoloExists(int id)
        {
            return _context.Articoli.Any(e => e.IdArticolo == id);
        }

        public HttpContext GetHttpContext()
        {
            return HttpContext;
        }

        public void AggiungiAlCarrello(int id)
        {
            bool isExist = false;
            var articolo = _context.Articoli.Find(id);
            if (articolo != null)
            {
                var carrelloSession = HttpContext.Session.GetString("carrelloList");
                if (carrelloSession != null)
                {
                    List<Carrello> cart = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);
                    foreach (var item in cart)
                    {
                        if (item.Articolo.IdArticolo == id)
                        {

                            isExist = true;
                            item.Quantita += 1;
                            HttpContext.Session.SetString("carrelloList", JsonConvert.SerializeObject(cart));
                        }

                    }
                }
                if (isExist == false)
                {
                    Carrello carrello = new Carrello();
                    carrello.Articolo = articolo;
                    carrello.Quantita = 1;
                    List<Carrello> carrelloList = new List<Carrello>();
                    if (!string.IsNullOrEmpty(carrelloSession))
                    {
                        carrelloList = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);
                    }
                    carrelloList.Add(carrello);
                    HttpContext.Session.SetString("carrelloList", JsonConvert.SerializeObject(carrelloList));
                }


            }
        }

        public IActionResult MostraCarrello()
        {
            List<Carrello> carrelloList = new List<Carrello>();
            var carrelloSession = HttpContext.Session.GetString("carrelloList");
            if (!string.IsNullOrEmpty(carrelloSession))
            {
                carrelloList = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);
            }
            return View(carrelloList);
        }

        public IActionResult RimuoviDalCarrello(int id)
        {
            var carrelloSession = HttpContext.Session.GetString("carrelloList");
            if (carrelloSession != null)
            {
                List<Carrello> cart = JsonConvert.DeserializeObject<List<Carrello>>(carrelloSession);
                foreach (var item in cart)
                {
                    if (item.Articolo.IdArticolo == id)
                    {
                        cart.Remove(item);
                        HttpContext.Session.SetString("carrelloList", JsonConvert.SerializeObject(cart));
                        TempData["message"] = "Articolo rimosso dal carrello";
                        return RedirectToAction(nameof(MostraCarrello));
                    }
                }
            }
            TempData["error"] = "Articolo non trovato";
            return RedirectToAction(nameof(MostraCarrello));
        }
    }
}
