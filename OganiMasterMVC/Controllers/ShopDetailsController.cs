using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Models.Basket;
using OganiMasterMVC.Models.ShopDetails;

namespace OganiMasterMVC.Controllers
{
    public class ShopDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ShopDetailsController(AppDbContext context)
        {
            _context = context;
        }

        const string BASKETKEY = "Basket";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var listProduct = _context.Products
                .Include(pi => pi.ProductImages).FirstOrDefault(o => o.Id == id);
            var listDetailsProductImages = _context.ProductImages.Where(x => x.ProductId == id).ToList();

            if (listProduct == null) return NotFound();

            var list = new ShopDetailsViewModel
            {
                Product = listProduct,
                ProductImages = listDetailsProductImages
            };

            return View("Index", list);
        }
        public IActionResult AddToBasket(int id, int count = 1)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var basketItems = AddBasketItemToBAsket(id);
            var jsonBasket = JsonConvert.SerializeObject(basketItems);
            Response.Cookies.Append(BASKETKEY, jsonBasket, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });
            var basketViewModel = GetBasketViewModel(basketItems);
            return Json(basketViewModel);
        }
        public List<BasketCookieItemModel> AddBasketItemToBAsket(int id)
        {
            var basketItems = GetBasketItems();
            var basketItem = basketItems.FindIndex(x => x.ProductId == id);
            if (basketItem >= 0)
            {
                basketItems[basketItem].Count++;
            }
            else
            {
                basketItems.Add(new BasketCookieItemModel { ProductId = id });
            }
            return basketItems;
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
        public IActionResult InitBasket()
        {
            var basketItems = GetBasketItems();
            var basketViewModel = GetBasketViewModel(basketItems);
            return Json(basketViewModel);
        }
        public BasketViewModel GetBasketViewModel(List<BasketCookieItemModel> basketCookies)
        {
            var basketViewModel = new BasketViewModel();
            var basketItemViewModels = new List<BasketItemViewModel>();
            foreach (var item in basketCookies)
            {
                var products = _context.Products.Find(item.ProductId);
                if (products == null)
                    continue;

                basketItemViewModels.Add(new BasketItemViewModel
                {
                    Id = products.Id,
                    Name = products.Name,
                    count = item.Count,
                    Price = products.Price
                });

            }

            basketViewModel.BasketItems = basketItemViewModels;
            basketViewModel.Count = basketItemViewModels.Sum(x => x.count);
            basketViewModel.TotalPrice = basketItemViewModels.Sum(x => x.Price * x.count);

            return basketViewModel;
        }
    }
}
