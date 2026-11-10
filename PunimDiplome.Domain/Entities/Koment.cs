namespace PunimDiplome.Domain.Entities;

public class Koment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string IdentityUserId { get; set; } = string.Empty;
    public int? ProduktId { get; set; }
    public Produkt? Produkt { get; set; }
}