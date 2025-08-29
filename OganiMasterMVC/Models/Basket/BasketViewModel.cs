namespace OganiMasterMVC.Models.Basket
{
    public class BasketViewModel
    {
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BasketItemViewModel> BasketItems { get; set; } = new List<BasketItemViewModel>();
    }
}
