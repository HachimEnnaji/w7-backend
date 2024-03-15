using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Pizzeria.data;
using Pizzeria.Models;
using System.Security.Claims;

namespace Pizzeria.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public LoginController(ApplicationDbContext context, IAuthenticationSchemeProvider schemeProvider)
        {
            _db = context;
            _schemeProvider = schemeProvider;
        }
        //GET
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Utente utente)
        {
            Utente? user = _db.Utenti.SingleOrDefault(x => x.Username == utente.Username && x.Password == utente.Password);

            if (user == null)
            {
                TempData["error"] = "Non esiste questo account";
                return View();
            }

            if (user.Username == "admin" && user.Password == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Ruolo),
                    new Claim(ClaimTypes.NameIdentifier, user.IdUtente.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                TempData["success"] = "Sezione Admin";
                return RedirectToAction("Index", "Home");
            }
            else if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.IdUtente.ToString())

                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                TempData["message"] = "Login effettuato";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Password sbagliata";
            }

            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            TempData["message"] = "Logout effettuato";
            return RedirectToAction("Index", "Home");

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Username, Password")] Utente utente)
        {

            var user = _db.Utenti.SingleOrDefault(x => x.Username == utente.Username);
            if (user != null)
            {
                TempData["error"] = "Username già esistente";
                return View();
            }
            ModelState.Remove("Ordini");
            if (ModelState.IsValid)
                try
                {


                    _db.Utenti.Add(utente);

                    _db.SaveChanges();
                    TempData["message"] = "Cliente aggiunto con successo";
                    //ritorna a Home Index
                    return RedirectToAction("Index", "Home");
                }

                catch (Exception ex)
                {
                    TempData["error"] = "Errore nella creazione dell'account, riprovare";

                    System.Diagnostics.Debug.WriteLine("ERROREEE" + ex.Message);
                }
            return View(utente);
        }
    }
}

