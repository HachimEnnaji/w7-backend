using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria.Models
{
    [NotMapped]
    public class Carrello
    {
        public Articolo Articolo { get; set; }
        public int Quantita { get; set; }

    }
}
