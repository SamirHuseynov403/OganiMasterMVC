namespace OganiMasterMVC.Data.Entities
{
    public class Tag : Base
    {
        public string? Name { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
