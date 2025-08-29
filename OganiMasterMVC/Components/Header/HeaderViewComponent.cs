using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Models.Basket;
using OganiMasterMVC.Models.Header;

namespace OganiMasterMVC.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        const string BASKETKEY = "Basket";
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listBio = _context.Bios.FirstOrDefault();
            var listSocial = _context.Socials.ToList();
            var listBasket = GetBasketItems();

            var list = new HeaderViewModel
            {
                Bio = listBio,
                Socials = listSocial,
                CountBasket = listBasket.Sum(x => x.Count)
            };

            return View(list);
        }
        public List<BasketCookieItemModel> GetBasketItems()
        {
            var basketItemsInString = Request.Cookies[BASKETKEY];
            var basketItems = new List<BasketCookieItemModel>();
            if (!string.IsNullOrEmpty(basketItemsInString))
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketCookieItemModel>>(basketItemsInString) ?? [];
            }
            return basketItems;
        }
    }
}
