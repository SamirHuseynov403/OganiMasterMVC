using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations.Schema;

namespace OganiMasterMVC.Data.Entities
{
    public class Category : Base
    {
        public string? Name { get; set; }
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public string? Url { get; set; }
        public List<Product>? Products { get; set; }
    }

}
