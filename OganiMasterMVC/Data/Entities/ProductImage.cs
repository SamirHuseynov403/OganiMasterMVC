namespace OganiMasterMVC.Data.Entities
{
    public class ProductImage : Base
    {
        public string? ImageUrl { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

    }

}
