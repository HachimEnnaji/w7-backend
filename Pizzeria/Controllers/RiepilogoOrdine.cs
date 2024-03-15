using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.data;
using System.Security.Claims;

namespace Pizzeria.Controllers
{
    public class RiepilogoOrdine : Controller
    {
        private readonly ApplicationDbContext _context;
        public RiepilogoOrdine(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ListaOrdini = await _context.Ordini.Include(o => o.DettagliOrdini).ThenInclude(dettaglio => dettaglio.Articoli).Where(o => o.IdUtente == userId).ToListAsync();

            return View(ListaOrdini);
        }
    }
}
