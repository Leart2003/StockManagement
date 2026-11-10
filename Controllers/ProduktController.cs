using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Punim_Diplome.Models;
using PunimDiplome.Application.Dtos;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;
using System.Security.Claims;

namespace Punim_Diplome.Controllers;

public class ProduktController : Controller
{
    private readonly IProduktService _produktService;
    private readonly IKomentService _komentService;
    private readonly IUserAdminService _userAdminService;

    public ProduktController(
        IProduktService produktService,
        IKomentService komentService,
        IUserAdminService userAdminService)
    {
        _produktService = produktService;
        _komentService = komentService;
        _userAdminService = userAdminService;
    }

    // ... Index, Menaxhment, Create, Edit, Delete, ExportToExcel, ToInput unchanged ...

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var produkt = await _produktService.GetByIdAsync(id.Value);
        if (produkt == null)
            return NotFound();

        var viewModel = new ProduktDetailsVM
        {
            Produkt = produkt,
            Koments = await _komentService.GetForProduktAsync(id.Value)
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int produktId, string content)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        await _komentService.AddAsync(produktId, content, userId);
        return RedirectToAction(nameof(Details), new { id = produktId });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var result = await _komentService.DeleteAsync(commentId, userId);

        return result.Status switch
        {
            KomentDeleteStatus.NotFound => NotFound(),
            KomentDeleteStatus.Forbidden => Forbid(),
            _ => result.ProduktId == null
                    ? RedirectToAction("Index", "Home")
                    : RedirectToAction(nameof(Details), new { id = result.ProduktId })
        };
    }

    [Authorize(Policy = "AdminEmail")]
    public async Task<IActionResult> PrivateAdmin()
    {
        var users = await _userAdminService.GetAllAsync();
        return View(users);
    }

    [Authorize(Policy = "AdminEmail")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userAdminService.DeleteAsync(id);
        return RedirectToAction(nameof(PrivateAdmin));
    }
}