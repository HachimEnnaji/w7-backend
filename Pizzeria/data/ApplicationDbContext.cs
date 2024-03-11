using Microsoft.EntityFrameworkCore;

namespace Pizzeria.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Articolo> Articoli { get; set; }
        public DbSet<Models.Utente> Utenti { get; set; }
        public DbSet<Models.Ordine> Ordini { get; set; }
        public DbSet<Models.DettagliOrdine> DettagliOrdini { get; set; }

    }
}
