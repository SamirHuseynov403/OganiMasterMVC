using Microsoft.AspNetCore.Mvc;
using OganiMasterMVC.Data.DataContext;
using OganiMasterMVC.Models.Footer;

namespace OganiMasterMVC.Components.Footer
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listBio = _context.Bios.FirstOrDefault();
            var list = new FooterViewModel
            {
                Bio = listBio
            };
            return View(list);
        }
    }
}
