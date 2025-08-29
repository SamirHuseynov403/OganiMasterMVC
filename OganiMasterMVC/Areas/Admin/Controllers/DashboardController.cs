using Microsoft.AspNetCore.Mvc;

namespace OganiMasterMVC.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
