namespace OganiMasterMVC.Data.Entities
{
    public class FeaturedProduct : Base
    {
        public int? ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
