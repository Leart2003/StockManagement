namespace PunimDiplome.Application.Dtos;

public class KomentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string IdentityUserId { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public int? ProduktId { get; set; }
}