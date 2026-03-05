using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuraPerfumes.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ICartRepository cartRepo, UserManager<IdentityUser> userManager)
        {
            _cartRepo = cartRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddItem(int perfumeId, int variantId, int qty = 1, int redirect = 0)
        {
            var userId = _userManager.GetUserId(User)!;

            int cartCount = await _cartRepo.AddItem(perfumeId, qty, userId);

            if (redirect == 0) return Json(cartCount);

            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int perfumeId, int variantId)
        {
            var cartCount = await _cartRepo.RemoveItem(perfumeId, variantId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Ok(0);

            int cartItem = await _cartRepo.GetCartItemCount(userId);
            return Ok(cartItem);
        }
    }
}
