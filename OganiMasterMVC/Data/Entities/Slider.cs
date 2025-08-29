namespace OganiMasterMVC.Data.Entities
{
    public class Slider : Base
    {
        public string? ImageUrl { get; set; }
        public string? Title { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }

}
