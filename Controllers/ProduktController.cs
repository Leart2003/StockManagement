using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PunimDiplome.Application.Dtos;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;

namespace Punim_Diplome.Controllers;

public class ProduktController : Controller
{
    private readonly IProduktService _produktService;

    public ProduktController(IProduktService produktService)
    {
        _produktService = produktService;
    }

    public async Task<IActionResult> Index(string? searchString, string? brand, string? selectedCategory)
    {
        var result = await _produktService.GetProductsAsync(searchString, brand, selectedCategory);

        var viewModel = new ProductVM
        {
            Products = result.Products,
            SearchString = searchString,
            SelectedBrand = brand,
            AvailableBrands = result.AvailableBrands,
            SelectedCategory = selectedCategory,
            AvailableCategories = result.AvailableCategories
        };

        return View(viewModel);
    }

    [Authorize(Policy = "AdminEmail")]
    public async Task<IActionResult> Menaxhment(string? searchBar)
    {
        var result = await _produktService.GetProductsAsync(searchBar, null, null);
        return View(result.Products);
    }

    [Authorize(Policy = "AdminEmail")]
    [HttpGet]
    public IActionResult Create() => View();

    [Authorize(Policy = "AdminEmail")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProduktDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await using var stream = dto.ImageFile?.OpenReadStream();
        await _produktService.CreateAsync(ToInput(dto), stream, dto.ImageFile?.FileName);

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "AdminEmail")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var produkt = await _produktService.GetByIdAsync(id);
        if (produkt == null)
            return RedirectToAction(nameof(Index));

        var dto = new ProduktDto
        {
            Id = produkt.Id,
            Name = produkt.Name,
            Brand = produkt.Brand,
            Ram = produkt.Ram,
            Description = produkt.Description,
            Date = produkt.Date,
            Price = produkt.Price,
            Storage = produkt.Storage,
            Procesor = produkt.Procesor,
            ScreenInch = produkt.ScreenInch,
            Storagetype = produkt.Storagetype,
            Category = produkt.Category
        };

        ViewData["ImageFileName"] = produkt.ImageFileName;
        ViewData["Produkt.Id"] = produkt.Id;
        return View(dto);
    }

    [Authorize(Policy = "AdminEmail")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProduktDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await using var stream = dto.ImageFile?.OpenReadStream();
        var updated = await _produktService.UpdateAsync(id, ToInput(dto), stream, dto.ImageFile?.FileName);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "AdminEmail")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _produktService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var produkt = await _produktService.GetDetailsAsync(id.Value);
        if (produkt == null)
            return NotFound();

        var viewModel = new ProduktDetailsVM
        {
            Produkt = produkt,
            Koments = produkt.Koments.ToList()
        };

        return View(viewModel);
    }

    [Authorize(Policy = "AdminEmail")]
    public async Task<IActionResult> ExportToExcel()
    {
        var (content, fileName) = await _produktService.ExportAsync();
        return File(content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    private static ProduktInput ToInput(ProduktDto dto) => new()
    {
        Name = dto.Name,
        Brand = dto.Brand,
        Description = dto.Description,
        Ram = dto.Ram,
        Price = dto.Price,
        Date = dto.Date,
        Storage = dto.Storage,
        Storagetype = dto.Storagetype,
        ScreenInch = dto.ScreenInch,
        Procesor = dto.Procesor,
        Category = dto.Category
    };
}