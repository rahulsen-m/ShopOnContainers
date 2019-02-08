using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoesOnContainer.Web.ClientApp.Infrastructure
{
    /// <summary>
    /// Helps to get the Catalog API base path to perform all the action provided by the Catalog API
    /// </summary>
    public class ApiPath
    {
        public static class Catalog
        {
            /// <summary>
            /// Get the API path for getting list of products based on the brand / type
            /// </summary>
            /// <param name="baseUri">Base Uri</param>
            /// <param name="NumberOfPage">Number of page index</param>
            /// <param name="ItemsToTake">Item to take in the result</param>
            /// <param name="brand">Product brand</param>
            /// <param name="type">Product type</param>
            /// <returns>API path for getting list of products based on condition</returns>
            public static string GetAllCatalogItems(string baseUri, int NumberOfPageIndex, int ItemsToTake, int? brand, int? type)
            {
                var filterQs = "";

                if (brand.HasValue || type.HasValue)
                {
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    var typeQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs = $"/type/{typeQs}/brand/{brandQs}";
                }

                return $"{baseUri}items{filterQs}?pageIndex={NumberOfPageIndex}&pageSize={ItemsToTake}";
            }

            /// <summary>
            /// Get the API path for getting the product details
            /// </summary>
            /// <param name="baseUri">Base Uri</param>
            /// <param name="id">Product Id</param>
            /// <returns>Path to get the product details</returns>
            public static string GetCatalogItem(string baseUri, int id) => $"{baseUri}/items/{id}";

            /// <summary>
            /// Get the API path for getting all the brands
            /// </summary>
            /// <param name="baseUri">Base Uri</param>
            /// <returns>Path to get all the brands</returns>
            public static string GetAllBrands(string baseUri) => $"{baseUri}catalogBrands";

            /// <summary>
            /// Get the API path for getting all the types
            /// </summary>
            /// <param name="baseUri">Base Uri</param>
            /// <returns>Path to get all the types</returns>
            public static string GetAllTypes(string baseUri) => $"{baseUri}catalogTypes";
        }
    }
}
