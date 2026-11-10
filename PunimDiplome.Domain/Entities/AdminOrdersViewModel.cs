using Microsoft.AspNetCore.Identity;
namespace PunimDiplome.Domain.Entities
{
    public class AdminOrdersViewModel
    {

        public List<OrderProduct> Orders { get; set; }
        public List<IdentityUser> Users { get; set; }


    }

}