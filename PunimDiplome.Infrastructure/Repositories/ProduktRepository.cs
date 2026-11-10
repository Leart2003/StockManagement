using Microsoft.EntityFrameworkCore;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;
using PunimDiplome.Infrastructure.Data;

namespace PunimDiplome.Infrastructure.Repositories;

public class ProduktRepository : IProduktRepository
{
    private readonly ApplicationDbContext _context;

    public ProduktRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Produkt> Query() => _context.Produktet.AsNoTracking();

    public Task<Produkt?> GetByIdAsync(int id) =>
        _context.Produktet.FirstOrDefaultAsync(p => p.Id == id);

    public Task<Produkt?> GetWithCommentsAsync(int id) =>
        _context.Produktet
            .Include(p => p.Koments)
            .FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<string>> GetBrandsAsync() =>
        _context.Produktet.Select(p => p.Brand).Distinct().ToListAsync();

    public Task<List<string?>> GetCategoriesAsync() =>
        _context.Produktet.Select(p => p.Category).Distinct().ToListAsync();

    public async Task AddAsync(Produkt produkt)
    {
        _context.Produktet.Add(produkt);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Produkt produkt)
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Produkt produkt)
    {
        _context.Produktet.Remove(produkt);
        await _context.SaveChangesAsync();
    }
}