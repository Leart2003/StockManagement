using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Application.Dtos
{
    public class ProduktInput
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Ram { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int Storage { get; set; }
        public string? Storagetype { get; set; }
        public double ScreenInch { get; set; }
        public string? Procesor { get; set; }
        public string? Category { get; set; }
    }
}
