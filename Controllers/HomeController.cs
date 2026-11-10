using Microsoft.AspNetCore.Mvc;
using Punim_Diplome.Application.Interfaces;
using Punim_Diplome.Models;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;
using System.Diagnostics;

namespace Punim_Diplome.Controllers;

public class HomeController : Controller
{
    private readonly IProduktService _produktService;

    public HomeController(IProduktService produktService)
    {
        _produktService = produktService;
    }

    public async Task<IActionResult> Index(string? searchString, string? brand)
    {
        var result = await _produktService.GetProductsAsync(searchString, brand, null);

        var viewModel = new ProductVM
        {
            Products = result.Products,
            SearchString = searchString,
            SelectedBrand = brand,
            AvailableBrands = result.AvailableBrands
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}