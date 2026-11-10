using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PunimDiplome.Application.Dtos;
using PunimDiplome.Application.Interfaces;

namespace PunimDiplome.Infrastructure.Services;

public class UserAdminService : IUserAdminService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserAdminService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<List<UserDto>> GetAllAsync() =>
        _userManager.Users
            .Select(u => new UserDto { Id = u.Id, UserName = u.UserName, Email = u.Email })
            .ToListAsync();

    public async Task<bool> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}