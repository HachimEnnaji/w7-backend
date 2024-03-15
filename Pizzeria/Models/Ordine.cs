using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria.Models
{
    public class Ordine
    {
        [Key]
        public int IdOrdine { get; set; }
        [Required]
        [ForeignKey("Utente")]
        public int IdUtente { get; set; }

        [Required]
        public string Indirizzo { get; set; }

        [Required]
        [Display(Name = "Consegnato")]
        public bool IsConsegnato { get; set; } = false;

        public string Note { get; set; } = "";


        public double? PrezzoTotale { get; set; }

        public DateTime? DataOrdine { get; set; }

        public virtual Utente Utente { get; set; }
        public virtual ICollection<DettagliOrdine> DettagliOrdini { get; set; }


    }
}
