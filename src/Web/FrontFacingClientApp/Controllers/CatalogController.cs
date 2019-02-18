using FrontFacingClientApp.Models;
using Microsoft.AspNetCore.Mvc;
using ShoesOnContainer.Web.ClientApp.Services;
using ShoesOnContainer.Web.ClientApp.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FrontFacingClientApp.Controllers
{
    /// <summary>
    /// Get product details
    /// </summary>
    public class CatalogController : Controller
    {
        private ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService) =>
            _catalogService = catalogService;

        public async Task<IActionResult> Index(int? BrandFilterApplied, int? TypesFilterApplied, int? page)
        {
            int itemsPage = 9;
            var catalog = await _catalogService.GetCatalogItems(page ?? 0, itemsPage, BrandFilterApplied, TypesFilterApplied);
            var result = new CatalogIndexViewModel()
            {
                CatalogItems = catalog.Data,
                Brands = await _catalogService.GetBrands(),
                Types = await _catalogService.GetTypes(),
                BrandFilterApplied = BrandFilterApplied ?? 0,
                TypesFilterApplied = TypesFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsPage, //catalog.Data.Count,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)catalog.Count / itemsPage))
                }
            };

            result.PaginationInfo.Next = (result.PaginationInfo.ActualPage == result.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            result.PaginationInfo.Previous = (result.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
