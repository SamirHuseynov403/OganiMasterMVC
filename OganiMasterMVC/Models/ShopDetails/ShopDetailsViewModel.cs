using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Models.ShopDetails
{
    public class ShopDetailsViewModel
    {
        public Product Product { get; set; } = new();
        public List<ProductImage> ProductImages { get; set; } = new();
    }
}
