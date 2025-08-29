using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiMasterMVC.Areas.Admin.Models;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            var existProduct = _context.Products.Any(c => c.Name!.ToLower() == product.Name!.ToLower());
            if (existProduct)
            {
                ModelState.AddModelError("Name", "Bu Product bazada movcuddur.");
                return View(product);
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
