using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria.Models
{
    public class DettagliOrdine
    {
        [Key]
        public int IdDettagliOrdine { get; set; }
        [Required]
        [ForeignKey("Ordine")]
        public int IdOrdine { get; set; }
        [Required]
        [ForeignKey("Articoli")]
        public int IdArticolo { get; set; }
        public int Quantita { get; set; }
        [Display(Name = "Prezzo unitario")]
        public double PrezzoUnitario { get; set; }

        public virtual Ordine Ordine { get; set; }
        public virtual Articolo Articoli { get; set; }
    }
}
