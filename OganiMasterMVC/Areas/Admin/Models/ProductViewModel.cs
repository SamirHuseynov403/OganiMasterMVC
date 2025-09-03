using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; } = null!;
        public IFormFile ImageFile { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public Product Product { get; set; } = new Product();
        public List<Product> Products { get; set; } = new List<Product>();
        public int? CategoryId { get; set; }
        public int? ProductTagId { get; set; }

        public List<SelectListItem> ListCategory { get; set; } = new List<SelectListItem>();
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public int ProductId { get; set; }          // <input hidden>
        public List<int> TagIds { get; set; } = new(); // <checkbox>
        public string? NewTagName { get; set; }     // <input>
    }
}
