using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Models.Home
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Product> ProductsChunked { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public Bio? Bio { get; set; } = new();
        public List<FeaturedProduct> FeaturedProducts { get; set; } = new();
        public int CountBasket { get; set; } = 0;
    }
}
