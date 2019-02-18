using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogApi.Domain
{
    public class CatalogItem
    {
        /// <summary>
        /// Get or Set Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or Set Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or Set Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or Set Price
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Get or Set PictureFileName
        /// </summary>
        public string PictureFileName { get; set; }

        /// <summary>
        /// Get or Set PictureUrl
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Get or Set CatalogTypeId
        /// </summary>
        public int CatalogTypeId { get; set; }

        /// <summary>
        /// Get or Set CatalogBrandId
        /// </summary>
        public int CatalogBrandId { get; set; }

        public CatalogBrand CatalogBrand { get; set; }
        public CatalogType CatalogType { get; set; }
    }
}
