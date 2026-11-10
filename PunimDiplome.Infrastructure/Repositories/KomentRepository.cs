using Microsoft.EntityFrameworkCore;
using PunimDiplome.Application.Dtos;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;
using PunimDiplome.Infrastructure.Data;

namespace PunimDiplome.Infrastructure.Repositories;

public class KomentRepository : IKomentRepository
{
    private readonly ApplicationDbContext _context;

    public KomentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<KomentDto>> GetByProduktIdAsync(int produktId) =>
        (from k in _context.Comments
         where k.ProduktId == produktId
         join u in _context.Users on k.IdentityUserId equals u.Id
         orderby k.Id
         select new KomentDto
         {
             Id = k.Id,
             Content = k.Content,
             IdentityUserId = k.IdentityUserId,
             UserName = u.UserName,
             ProduktId = k.ProduktId
         }).ToListAsync();

    public Task<Koment?> GetByIdAsync(int id) =>
        _context.Comments.FirstOrDefaultAsync(k => k.Id == id);

    public async Task AddAsync(Koment koment)
    {
        _context.Comments.Add(koment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Koment koment)
    {
        _context.Comments.Remove(koment);
        await _context.SaveChangesAsync();
    }
}