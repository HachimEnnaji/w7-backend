using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Utente
    {
        [Key]
        public int IdUtente { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Ruolo { get; set; } = "User";

        public virtual ICollection<Ordine> Ordini { get; set; }
    }
}
