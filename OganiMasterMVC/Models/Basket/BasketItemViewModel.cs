namespace OganiMasterMVC.Models.Basket
{
    public class BasketItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int count { get; set; } = 1;
        public decimal Price { get; set; }
    }
}
