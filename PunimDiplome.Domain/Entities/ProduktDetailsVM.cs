using System.ComponentModel.DataAnnotations;

namespace PunimDiplome.Domain.Entities
{
    public class ProduktDetailsVM
    {
        public Produkt? Produkt { get; set; }
        public List<Koment>? Koments { get; set; } = new List<Koment>();

        [Required(ErrorMessage = "Comment content is required.")]
        public string NewKoment { get; set; }= string.Empty;
    }
}
