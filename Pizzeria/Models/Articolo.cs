using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Articolo
    {
        [Key]
        public int IdArticolo { get; set; }

        [Required]
        public string Nome { get; set; }
        [Required]
        public string Immagine { get; set; }
        [Required]
        public double Prezzo { get; set; }
        [Required]
        [Display(Name = "Tempo di consegna")]
        public int TempoConsegna { get; set; }
        [Required]
        public string Ingredienti { get; set; }

        public virtual ICollection<DettagliOrdine> DettagliOrdini { get; set; }


    }
}
