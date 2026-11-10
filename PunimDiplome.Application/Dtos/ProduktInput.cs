namespace PunimDiplome.Application.Dtos
{
    public class ProduktInput
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Ram { get; set; }
        public decimal Price { get; set; }
        public DateOnly Date { get; set; }
        public string? Storage { get; set; }
        public string Storagetype { get; set; } = string.Empty;
        public int ScreenInch { get; set; }
        public string? Procesor { get; set; }
        public string? Category { get; set; }
    }
}
