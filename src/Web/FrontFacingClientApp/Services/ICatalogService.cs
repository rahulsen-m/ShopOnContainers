using Microsoft.AspNetCore.Mvc.Rendering;
using ShoesOnContainer.Web.ClientApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoesOnContainer.Web.ClientApp.Services
{
    /// <summary>
    /// Get result from product catalog API
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Get list of products
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="take">Number of items</param>
        /// <param name="brand">brand id</param>
        /// <param name="type">type id</param>
        /// <returns>List of products</returns>
        Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type);

        /// <summary>
        /// Get all brands
        /// </summary>
        /// <returns>List of brands</returns>
        Task<IEnumerable<SelectListItem>> GetBrands();

        /// <summary>
        /// Get all types
        /// </summary>
        /// <returns>List of types</returns>
        Task<IEnumerable<SelectListItem>> GetTypes();
    }
}
