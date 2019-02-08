﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using ProductCatalogApi.Domain;
using ProductCatalogApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        // injected a lifetime instance of CatalogSettings via IOptionsSnapshot
        private readonly IOptionsSnapshot<CatalogSettings> _settings;

        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = catalogContext;
            _settings = settings;
            // Stop track the db changes 
            // As this is a read only context so we don't not need to track the changes
            catalogContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        /// Get all the catalog types
        /// </summary>
        /// <returns>List of catalog types</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _catalogContext.CatalogTypes.ToListAsync();
            return Ok(items);

        }

        /// <summary>
        /// Get all the catalog brands
        /// </summary>
        /// <returns>List of catalog brands</returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _catalogContext.CatalogBrands.ToListAsync();
            return Ok(items);

        }

        /// <summary>
        /// Get the product by id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Product details</returns>
        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced", _settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }
            return NotFound();
        }

        /// <summary>
        /// Get products for pagination
        /// </summary>
        /// <param name="pageSize">Number of product per result</param>
        /// <param name="pageIndex">Index of the product list</param>
        /// <returns>List of products for pagination</returns>
        //GET api/Catalog/items[?pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                              .LongCountAsync();
            var itemsOnPage = await _catalogContext.CatalogItems
                              .OrderBy(c => c.Name)
                              .Skip(pageSize * pageIndex)
                              .Take(pageSize)
                              .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);

        }

        /// <summary>
        /// Get product with name
        /// </summary>
        /// <param name="name">Name of the product</param>
        /// <param name="pageSize">Number of the product per result</param>
        /// <param name="pageIndex">Index of the product list</param>
        /// <returns>Products with given name</returns>
        //GET api/Catalog/items/withname/Wonder?pageSize=2&pageIndex=0
        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _catalogContext.CatalogItems
                               .Where(c => c.Name.StartsWith(name))
                              .LongCountAsync();
            var itemsOnPage = await _catalogContext.CatalogItems
                              .Where(c => c.Name.StartsWith(name))
                              .OrderBy(c => c.Name)
                              .Skip(pageSize * pageIndex)
                              .Take(pageSize)
                              .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);

        }

        /// <summary>
        /// Get product of the same brand
        /// </summary>
        /// <param name="catalogTypeId">Product type</param>
        /// <param name="catalogBrandId">Product brand</param>
        /// <param name="pageSize">Number of product per result</param>
        /// <param name="pageIndex">Index of the product list</param>
        /// <returns>List of product of the same brand</returns>
        // GET api/Catalog/Items/type/1/brand/null[?pageSize=4&pageIndex=0]
        [HttpGet]
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        public async Task<IActionResult> Items(int? catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            // IQueryable used to indicate that the query is not ready yet and we will not make db call
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;

            if (catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }
            if (catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId);
            }

            var totalItems = await root.LongCountAsync();
            var itemsOnPage = await root
                              .OrderBy(c => c.Name)
                              .Skip(pageSize * pageIndex)
                              .Take(pageSize)
                              .ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);

        }

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="product">Product details</param>
        /// <returns>Newly created product details</returns>
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct([FromBody] CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };
            _catalogContext.CatalogItems.Add(item);
            await _catalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id });
        }


        /// <summary>
        /// Update the existing product
        /// </summary>
        /// <param name="productToUpdate">Product details</param>
        /// <returns>Newly updated product details</returns>
        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await _catalogContext.CatalogItems
                              .SingleOrDefaultAsync(i => i.Id == productToUpdate.Id);
            if (catalogItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });
            }
            catalogItem = productToUpdate;
            _catalogContext.CatalogItems.Update(catalogItem);
            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.Id });
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>204 status code</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _catalogContext.CatalogItems.SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();

            }
            _catalogContext.CatalogItems.Remove(product);
            await _catalogContext.SaveChangesAsync();
            return NoContent();

        }

        #region Private Methods
        // TODO: Move to helper class
        private List<CatalogItem> ChangeUrlPlaceHolder(List<CatalogItem> items)
        {
            items.ForEach(
                x => x.PictureUrl = x.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                _settings.Value.ExternalCatalogBaseUrl));
            return items;
        }

        #endregion
    }
}