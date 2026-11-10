namespace PunimDiplome.Domain.Entities;

public class Produkt
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Description { get; set; }
    public int Ram { get; set; }
    public decimal Price { get; set; }
    public DateOnly Date { get; set; }
    public string? Storage { get; set; }
    public string Storagetype { get; set; } = string.Empty;
    public int ScreenInch { get; set; }
    public string? Procesor { get; set; }
    public string? Category { get; set; }
    public string? ImageFileName { get; set; }

    public ICollection<Koment> Koments { get; set; } = new List<Koment>();
}