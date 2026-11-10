using PunimDiplome.Application.Dtos;

namespace PunimDiplome.Application.Interfaces;

public interface IUserAdminService
{
    Task<List<UserDto>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
}