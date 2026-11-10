using PunimDiplome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Application.Interfaces
{
    public interface IProduktExporter
    {
      
        byte[] ExportToExcel(IEnumerable<Produkt> produktet);
    }
}
