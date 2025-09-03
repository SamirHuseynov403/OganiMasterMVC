using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Models;
using OganiMasterMVC.Models.Basket;
using OganiMasterMVC.Models.Home;
using System.Diagnostics;

namespace OganiMasterMVC.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        const string BASKETKEY = "Basket";
        public IActionResult Index()
        {
            var listProduct = _context.Products.ToList();
            var listProductFeatured = _context.FeaturedProducts
                .Include(p=>p.Product)
                .ToList();
            var listChunk = listProduct
                .Take(listProduct.Count).ToList();
            var listCategory = _context.Categories.ToList();
            var listBio = _context.Bios.FirstOrDefault();
            var list = new HomeViewModel
            {
                Products = listProduct,
                Categories = listCategory,
                Bio = listBio,
                ProductsChunked = listChunk,
                FeaturedProducts = listProductFeatured,
            };

            return View(list);
        }

       
    }
}
