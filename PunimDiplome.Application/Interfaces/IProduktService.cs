using PunimDiplome.Domain.Entities;
using PunimDiplome.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Application.Interfaces
{
    public interface IProduktService
    {
        Task<ProduktListResult> GetProductsAsync(string? searchString, string? brand, string? category);
        Task<Produkt?> GetByIdAsync(int id);
        Task<Produkt?> GetDetailsAsync(int id);
        Task CreateAsync(ProduktInput input, Stream? image, string? imageName);
        Task<bool> UpdateAsync(int id, ProduktInput input, Stream? image, string? imageName);
        Task<bool> DeleteAsync(int id);
        Task<(byte[] Content, string FileName)> ExportAsync();
    }
}
