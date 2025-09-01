using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OganiMasterMVC.Areas.Admin.Models;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Areas.Admin.Controllers
{
    public class ProductController : AdminController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var listProducts = _context.Products
                .Include(c => c.Category).ToList();

            return View(listProducts);
        }
        public IActionResult Create()
        {
            var listCategory = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            var list = new ProductViewModel
            {
                ListCategory = listCategory
            };
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListCategory = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(model);
            }

            // Fayl yükləmə
            if (model.Product.ImageFile != null && model.Product.ImageFile.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath, "img", "product");
                Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Product.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Product.ImageFile.CopyToAsync(stream);
                }

                model.Product.ImageUrl = $"/img/product/{fileName}";
            }

            _context.Products.Add(model.Product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var listCategory = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = (c.Id == product.CategoryId)
                })
                .ToList();

            var vm = new ProductViewModel
            {
                Product = product,
                ListCategory = listCategory
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            var existProduct = _context.Products.AsNoTracking().First(x => x.Id == id);
            if (existProduct == null) return BadRequest();
            var existProductByName = _context.Products.Any(c => c.Name!.ToLower() == product.Name!.ToLower() && c.Id != id);

            if (existProductByName)
            {
                ModelState.AddModelError("Name", "Bu product bazada movcuddur.");
                return View(product);
            }
            if (product.Name is not null && product.ImageUrl?.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath, "img", "product");
                Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(product.ImageFile?.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await product.ImageFile.CopyToAsync(stream);

                product.ImageUrl = $"/img/product/{fileName}";
            }

            existProduct.Name = product.Name;
            existProduct.ImageUrl = product.ImageUrl;


            _context.Products.Update(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
