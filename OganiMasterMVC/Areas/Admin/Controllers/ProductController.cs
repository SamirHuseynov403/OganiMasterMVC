using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OganiMasterMVC.Areas.Admin.Models;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Data.Entities;
using OganiMasterMVC.Areas.Admin.Data;

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
                .Include(c => c.Category)
                .Include(p => p.ProductTags)
                .ThenInclude(t => t.Tag)
                .Include(i => i.ProductImages)
                .ToList();

            var list = new ProductViewModel
            {
                Products = listProducts,
                Tags = _context.Tags.ToList()
            };

            return View(list);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ListCategory = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(model);
            }
            if (!model.ImageFile.IsImage())
            {
                model.ListCategory = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(model);
            }
            if (!model.ImageFile.IsAllowedSize(1))
            {
                model.ListCategory = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList();
                return View(model);
            }
            string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
            var unicalName = await model.ImageFile.GenerateFile(PathConstants.ProductImagePath, extension);

            var product = new Product
            {
                Name = model.Name,
                ImageUrl = unicalName,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Description = model.Description,
                DiscountPercent = model.DiscountPercent
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //if (model.Product.ImageFile != null && model.Product.ImageFile.Length > 0)
            //{
            //    var uploadsRoot = Path.Combine(_env.WebRootPath, "img", "product");
            //    Directory.CreateDirectory(uploadsRoot);

            //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.Product.ImageFile.FileName)}";
            //    var filePath = Path.Combine(uploadsRoot, fileName);

            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await model.Product.ImageFile.CopyToAsync(stream);
            //    }

            //    model.Product.ImageUrl = $"/img/product/{fileName}";
            //}

            //_context.Products.Add(model.Product);
            //await _context.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));
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

            var vm = new UpdateProductViewModel
            {
                CoverImageUrl = product.ImageUrl,
                Name = product.Name!,
                Price = product.Price,
                Description = product.Description,
                DiscountPercent = product.DiscountPercent,
                CategoryId = product.CategoryId,
                ListCategory = listCategory
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateProductViewModel model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ListCategory = _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == model.CategoryId })
                    .ToList();
                return View(model);
            }

            // Fayl gəlibsə — VALIDASİYA
            if (model.ImageFile is not null && model.ImageFile.Length > 0)
            {
                if (!model.ImageFile.IsImage()) // şəkil DEYİLSƏ
                {
                    ModelState.AddModelError("ImageFile", "Yalnız şəkil faylı seçin.");
                    model.ListCategory = _context.Categories
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == model.CategoryId })
                        .ToList();
                    return View(model);
                }

                if (!model.ImageFile.IsAllowedSize(1)) // 1 MB-dan BÖYÜKDÜRSƏ
                {
                    ModelState.AddModelError("ImageFile", "Şəkilin ölçüsü maksimum 1 MB olmalıdır.");
                    model.ListCategory = _context.Categories
                        .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == model.CategoryId })
                        .ToList();
                    return View(model);
                }

                // Köhnə faylı sil
                if (!string.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    var oldNameOnly = Path.GetFileName(product.ImageUrl); // yalnız fayl adı
                    var oldPhysical = Path.Combine(PathConstants.ProductImagePath, oldNameOnly);
                    if (System.IO.File.Exists(oldPhysical))
                        System.IO.File.Delete(oldPhysical);
                }

                // Yenisini yaz
                var uniqueName = await model.ImageFile.GenerateFile(PathConstants.ProductImagePath);
                // DB üçün web yolunu saxla (relativ):
                product.ImageUrl = uniqueName;
            }

            // Qalan sahələr
            product.Name = model.Name!;
            product.Price = model.Price;
            product.Description = model.Description;
            product.DiscountPercent = model.DiscountPercent;
            product.CategoryId = model.CategoryId;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertTags(ProductTagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.NewTagName is not null)
            {
                var newTag = new Tag { Name = model.NewTagName };
                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync();
                model.TagIds.Add(newTag.Id);
                var newProductTag = new ProductTag
                {
                    ProductId = model.ProductId,
                    TagId = newTag.Id
                };
                _context.ProductTags.Add(newProductTag);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTag(int productId, int tagId)
        {
            var link = await _context.ProductTags
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.TagId == tagId);

            if (link != null)
            {
                _context.ProductTags.Remove(link);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertImages(ProductImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.ImageFile is not null)
            {
                
                string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
                var unicalName = await model.ImageFile.GenerateFile(PathConstants.ProductImagePath, extension);

                var newTag = new ProductImage { ImageUrl = unicalName, ProductId = model.ProductId };
                _context.ProductImages.Add(newTag);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            var link = await _context.ProductImages
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.Id == imageId);

            if (link != null)
            {
                _context.ProductImages.Remove(link);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
