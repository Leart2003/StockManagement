using System;
using System.Collections.Generic;
using System.Text;
using PunimDiplome.Domain.Entities;
namespace PunimDiplome.Application.Interfaces
{
    public interface IProduktRepository
    {
        IQueryable<Produkt> Query();
        Task<Produkt?> GetByIdAsync(int id);
        Task<Produkt?> GetWithCommentsAsync(int id);
        Task<List<string>> GetBrandsAsync();
        Task<List<string?>> GetCategoriesAsync();
        Task AddAsync(Produkt produkt);
        Task UpdateAsync(Produkt produkt);
        Task DeleteAsync(Produkt produkt);
    }
}
