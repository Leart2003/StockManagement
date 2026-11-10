using Microsoft.EntityFrameworkCore;
using PunimDiplome.Application.Dtos;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Application.Services
{
    public class ProduktService : IProduktService
    {
        private readonly IProduktRepository _repository;
        private readonly IImageStorage _imageStorage;
        private readonly IProduktExporter _exporter;

        public ProduktService(IProduktRepository repository, IImageStorage imageStorage, IProduktExporter exporter)
        {
            _repository = repository;
            _imageStorage = imageStorage;
            _exporter = exporter;
        }

        public async Task<ProduktListResult> GetProductsAsync(string? searchString, string? brand, string? category)
        {
            var query = _repository.Query().OrderByDescending(p => p.Id).AsQueryable();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand == brand);

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(p => p.Name.Contains(searchString));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category!.Contains(category));

            return new ProduktListResult
            {
                Products = await query.ToListAsync(),
                AvailableBrands = await _repository.GetBrandsAsync(),
                AvailableCategories = await _repository.GetCategoriesAsync()
            };
        }

        public Task<Produkt?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Produkt?> GetDetailsAsync(int id) => _repository.GetWithCommentsAsync(id);

        public async Task CreateAsync(ProduktInput input, Stream? image, string? imageName)
        {
            var produkt = new Produkt
            {
                Name = input.Name,
                Brand = input.Brand,
                Description = input.Description,
                Ram = input.Ram,
                Price = input.Price,
                Date = input.Date,
                Storage = input.Storage,
                Storagetype = input.Storagetype,
                ScreenInch = input.ScreenInch,
                Procesor = input.Procesor,
                Category = input.Category
            };

            if (image != null && imageName != null)
                produkt.ImageFileName = await _imageStorage.SaveAsync(image, imageName);

            await _repository.AddAsync(produkt);
        }

        public async Task<bool> UpdateAsync(int id, ProduktInput input, Stream? image, string? imageName)
        {
            var produkt = await _repository.GetByIdAsync(id);
            if (produkt == null) return false;

            produkt.Name = input.Name;
            produkt.Brand = input.Brand;
            produkt.Description = input.Description;
            produkt.Ram = input.Ram;
            produkt.Price = input.Price;
            produkt.Date = input.Date;
            produkt.Storage = input.Storage;
            produkt.Storagetype = input.Storagetype;
            produkt.ScreenInch = input.ScreenInch;
            produkt.Procesor = input.Procesor;
            produkt.Category = input.Category;

            if (image != null && imageName != null)
            {
                if (!string.IsNullOrEmpty(produkt.ImageFileName))
                    _imageStorage.Delete(produkt.ImageFileName);

                produkt.ImageFileName = await _imageStorage.SaveAsync(image, imageName);
            }

            await _repository.UpdateAsync(produkt);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var produkt = await _repository.GetByIdAsync(id);
            if (produkt == null) return false;

            if (!string.IsNullOrEmpty(produkt.ImageFileName))
                _imageStorage.Delete(produkt.ImageFileName);

            await _repository.DeleteAsync(produkt);
            return true;
        }

        public async Task<(byte[] Content, string FileName)> ExportAsync()
        {
            var produktet = await _repository.Query().ToListAsync();
            var content = _exporter.ExportToExcel(produktet);
            var fileName = $"Produktet_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return (content, fileName);
        }
    }
}
