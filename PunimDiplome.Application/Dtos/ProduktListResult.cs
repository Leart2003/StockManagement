using PunimDiplome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Application.Dtos
{
    public class ProduktListResult
    {

        public List<Produkt> Products { get; set; } = new();
        public List<string> AvailableBrands { get; set; } = new();
        public List<string?> AvailableCategories { get; set; } = new();
    }
}
