using System.ComponentModel.DataAnnotations.Schema;

namespace OganiMasterMVC.Data.Entities
{
    public class Product : Base
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountPercent { get; set; }

        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductTag> ProductTags { get; set; }= new List<ProductTag>();
        public List<FeaturedProduct>? FeaturedProducts { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

    }

}
