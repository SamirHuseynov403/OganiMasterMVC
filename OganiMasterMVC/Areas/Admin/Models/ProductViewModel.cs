using Microsoft.AspNetCore.Mvc.Rendering;
using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }=new Product();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<SelectListItem> ListCategory { get; set; } = new List<SelectListItem>();
    }
}
