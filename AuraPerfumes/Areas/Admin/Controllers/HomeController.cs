using Microsoft.AspNetCore.Mvc;

namespace AuraPerfumes.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
