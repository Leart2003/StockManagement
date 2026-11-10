using PunimDiplome.Application.Dtos;
using PunimDiplome.Domain.Entities;

namespace PunimDiplome.Application.Interfaces;

public interface IKomentRepository
{
    Task<List<KomentDto>> GetByProduktIdAsync(int produktId);
    Task<Koment?> GetByIdAsync(int id);
    Task AddAsync(Koment koment);
    Task DeleteAsync(Koment koment);
}