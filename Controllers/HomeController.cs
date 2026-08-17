using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Punim_Diplome.Models;
using System.Diagnostics;

namespace Punim_Diplome.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        /// <summary>
        /// Displays the list of products, optionally filtered by brand and/or a search term.
        /// </summary>
        /// <param name="searchString">
        /// Text typed by the user to search for in the product name. Optional — if null or empty, no name filter is applied.
        /// </param>
        /// <param name="brand">
        /// The brand selected by the user to filter by. Optional — if null or empty, products from all brands are shown.
        /// </param>
        /// <returns>
        /// The Index view, populated with the filtered product list, the selected filters,
        /// and the list of available brands (for the filter dropdown).
        /// </returns>
        public async Task<IActionResult> IndexAsync(String searchString, String brand)
        {
            var produktetquery = context.Produktet.OrderByDescending(p => p.Id).ToList();

            if (!string.IsNullOrEmpty(brand))
            {
                produktetquery = produktetquery.Where(p => p.Brand == brand).ToList();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                produktetquery = produktetquery.Where(p => p.Name!.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            var availableBrands = await context.Produktet
            .Select(p => p.Brand)
            .Distinct()
            .ToListAsync();


            var viewModel = new ProductVM()
            {
                Products = produktetquery.ToList(),
                SearchString = searchString,
                SelectedBrand = brand,
                AvailableBrands = availableBrands
            };


            return View(viewModel);
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
