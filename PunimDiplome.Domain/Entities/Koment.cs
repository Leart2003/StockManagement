using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PunimDiplome.Domain.Entities
{
    public class Koment
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        [Required]
        public string? IdentityUserId { get; set; }


        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }


        public int? ProduktId { get; set; }

        [ForeignKey("ProduktId")]
        public Produkt? Produkt { get; set; }


    }
}
