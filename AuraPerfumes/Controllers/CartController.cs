using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuraPerfumes.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public IActionResult AddItem(int perfumeId, int qty=1)
        {
            return View();
        }

        public IActionResult RemoveItem(int perfumeId)
        {
            return View();
        }

        public IActionResult GetUserCart()
        {
            return View();
        }
        public IActionResult GetTotalItemInCart()
        {
            return View();
        }
    }
}
