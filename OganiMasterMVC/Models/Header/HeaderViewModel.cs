using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Models.Header
{
    public class HeaderViewModel
    {
        public Bio? Bio { get; set; } = new();
        public List<Social> Socials { get; set; } = new();
        public int CountBasket { get; set; } = 0;
    }
}
