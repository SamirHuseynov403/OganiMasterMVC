using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Data.Entities;

namespace OganiMasterMVC.Areas.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var existCategory = _context.Categories.Any(c => c.Name!.ToLower() == category.Name!.ToLower());
            if (existCategory)
            {
                ModelState.AddModelError("Name", "Bu Category bazada movcuddur.");
                return View(category);
            }
            if (category.Name is not null && category.ImageFile?.Length > 0)
            {
                var uploadsRoot = Path.Combine(_env.WebRootPath, "img", "product");
                Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(category.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await category.ImageFile.CopyToAsync(stream);

                category.Url = $"/img/product/{fileName}";
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var existCategory = _context.Categories.AsNoTracking().First(x => x.Id == id);
            if(existCategory==null) return BadRequest();
            var existCategoryByName = _context.Categories.Any(c => c.Name!.ToLower() == category.Name!.ToLower() && c.Id != id);

            if (existCategoryByName)
            {
                ModelState.AddModelError("Name", "Bu Category bazada movcuddur.");
                return View(category);
            }

            existCategory.Name = category.Name;
            _context.Categories.Update(existCategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
