using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Punim_Diplome.Models;

public class ProduktDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Brand is required.")]
    public string? Brand { get; set; }

    [Range(1, 128, ErrorMessage = "RAM must be between 1 and 128 GB.")]
    public int Ram { get; set; }

    [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    public DateOnly Date { get; set; }
    public string? Storage { get; set; }

    [Required(ErrorMessage = "Storage type is required.")]
    public string? Storagetype { get; set; }

    [Range(10, 50, ErrorMessage = "Screen size must be between 10 and 50 inches.")]
    public int ScreenInch { get; set; }

    public string? Procesor { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }

    public IFormFile? ImageFile { get; set; }
}