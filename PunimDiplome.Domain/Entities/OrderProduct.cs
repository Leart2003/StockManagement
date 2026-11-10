using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PunimDiplome.Domain.Entities
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; } = "Pending";

        [ForeignKey("ProductId")]
        
        public virtual Produkt Produkt { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }

    }
}
