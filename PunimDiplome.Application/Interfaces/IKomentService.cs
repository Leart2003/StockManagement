using PunimDiplome.Application.Dtos;

namespace PunimDiplome.Application.Interfaces;

public interface IKomentService
{
    Task<List<KomentDto>> GetForProduktAsync(int produktId);
    Task<bool> AddAsync(int produktId, string content, string userId);
    Task<KomentDeleteResult> DeleteAsync(int commentId, string userId);
}