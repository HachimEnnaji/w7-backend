using Microsoft.AspNetCore.Mvc;
using Pizzeria.data;

namespace Pizzeria.Controllers
{

    public class SezioneAdmin : Controller
    {
        private readonly ApplicationDbContext _context;
        public SezioneAdmin(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OrdiniEvasi()
        {
            var totaleOrdini = _context.Ordini.Where(o => o.IsConsegnato == true).Count();
            return Json(totaleOrdini);
        }

        public IActionResult TotaleIncassodiOggi(int year, int month, int day)
        {
            try
            {
                DateOnly date = new DateOnly(year, month, day);
                var totaleIncasso = _context.Ordini.Where(o => DateOnly.FromDateTime((DateTime)o.DataOrdine) == date).Sum(o => o.PrezzoTotale);
                return Json(totaleIncasso);
            }
            catch (Exception ex)
            {
                // Log the exception


                // Return a generic error message
                return StatusCode(500, "An error occurred while processing the date status request.");
            }
        }
    }
}
