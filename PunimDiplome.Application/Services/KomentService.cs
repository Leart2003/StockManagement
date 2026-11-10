using PunimDiplome.Application.Dtos;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;

namespace PunimDiplome.Application.Services;

public class KomentService : IKomentService
{
    private readonly IKomentRepository _repository;

    public KomentService(IKomentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<KomentDto>> GetForProduktAsync(int produktId) =>
        _repository.GetByProduktIdAsync(produktId);

    public async Task<bool> AddAsync(int produktId, string content, string userId)
    {
        if (string.IsNullOrWhiteSpace(content))
            return false;

        var koment = new Koment
        {
            Content = content,
            IdentityUserId = userId,
            ProduktId = produktId
        };

        await _repository.AddAsync(koment);
        return true;
    }

    public async Task<KomentDeleteResult> DeleteAsync(int commentId, string userId)
    {
        var koment = await _repository.GetByIdAsync(commentId);

        if (koment == null)
            return new KomentDeleteResult(KomentDeleteStatus.NotFound, null);

        if (koment.IdentityUserId != userId)
            return new KomentDeleteResult(KomentDeleteStatus.Forbidden, koment.ProduktId);

        await _repository.DeleteAsync(koment);
        return new KomentDeleteResult(KomentDeleteStatus.Deleted, koment.ProduktId);
    }
}