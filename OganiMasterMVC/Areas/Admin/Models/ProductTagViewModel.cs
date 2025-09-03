namespace OganiMasterMVC.Areas.Admin.Models
{
    public class ProductTagViewModel
    {
        public int ProductId { get; set; }
        public string? NewTagName { get; set; }
        public List<int> TagIds { get; set; }
    }
    public class ProductImageViewModel
    {
        public int ProductId { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
        public List<int> TagIds { get; set; }
    }
}
