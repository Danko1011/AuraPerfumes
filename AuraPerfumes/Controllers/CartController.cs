using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuraPerfumes.Data;
using AuraPerfumes.Models;
using Microsoft.EntityFrameworkCore;
using AuraPerfumes.Models.DTOs;



namespace AuraPerfumes.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public CartController(ICartRepository cartRepo, UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _cartRepo = cartRepo;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> AddItem(int perfumeId, int variantId, int qty = 1, int redirect = 0)
        {
            var userId = _userManager.GetUserId(User);

            int cartCount = await _cartRepo.AddItem(perfumeId, variantId, qty, userId);

            if (redirect == 0)
                return Json(cartCount);

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

        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartRepo.GetUserCart();

            if (cart == null || cart.CartDetails == null || !cart.CartDetails.Any())
            {
                return RedirectToAction("GetUserCart");
            }

            var subtotal = cart.CartDetails.Sum(x => x.Variant.Price * x.Quantity);
            var shipping = GetCourierPrice("Speedy");

            var vm = new CheckoutVM
            {
                Cart = cart,
                CourierName = "Speedy",
                ShippingPrice = shipping,
                Discount = 0,
                Subtotal = subtotal,
                GrandTotal = subtotal + shipping,
                PaymentMethod = "CashOnDelivery"
            };

            return View(vm);
        }
        public async Task<IActionResult> OrderInfo()
        {
            var cart = await _cartRepo.GetUserCart();

            if (cart == null || cart.CartDetails == null || !cart.CartDetails.Any())
            {
                return RedirectToAction("GetUserCart");
            }

            var subtotal = cart.CartDetails.Sum(x => x.Variant.Price * x.Quantity);
            var shipping = GetCourierPrice("Speedy");

            var vm = new CheckoutVM
            {
                Cart = cart,
                CourierName = "Speedy",
                ShippingPrice = shipping,
                Discount = 0,
                Subtotal = subtotal,
                GrandTotal = subtotal + shipping,
                PaymentMethod = "CashOnDelivery"
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutVM model)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (model.PaymentMethod == "CardPayment")
            {
                if (string.IsNullOrWhiteSpace(model.CardHolderName))
                    ModelState.AddModelError("CardHolderName", "Card holder name is required.");

                if (string.IsNullOrWhiteSpace(model.CardNumber))
                    ModelState.AddModelError("CardNumber", "Card number is required.");

                if (string.IsNullOrWhiteSpace(model.ExpiryDate))
                    ModelState.AddModelError("ExpiryDate", "Expiry date is required.");

                if (string.IsNullOrWhiteSpace(model.CVV))
                    ModelState.AddModelError("CVV", "CVV is required.");
            }

            var cart = await _db.ShoppingCarts
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Perfume)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Variant)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartDetails == null || !cart.CartDetails.Any())
                return RedirectToAction("GetUserCart");

            var subtotal = cart.CartDetails.Sum(x => x.Variant.Price * x.Quantity);
            var shipping = GetCourierPrice(model.CourierName);
            var discount = GetDiscountFromPromoCode(model.PromoCode, subtotal);
            var grandTotal = subtotal + shipping - discount;

            if (grandTotal < 0)
                grandTotal = 0;

            model.Cart = cart;
            model.Subtotal = subtotal;
            model.ShippingPrice = shipping;
            model.Discount = discount;
            model.GrandTotal = grandTotal;

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}")
                    .ToList();

                ViewBag.DebugErrors = errors;
                return View("Checkout", model);
            }

            var pendingStatus = await _db.OrderStatuses.FirstOrDefaultAsync(s => s.StatusName == "Pending");

            if (pendingStatus == null)
            {
                pendingStatus = new OrderStatus
                {
                    Status = 1,
                    StatusName = "Pending"
                };

                _db.OrderStatuses.Add(pendingStatus);
                await _db.SaveChangesAsync();
            }

            var order = new Order
            {
                UserId = userId,
                OrderStatusId = pendingStatus.Id,
                CreateDate = DateTime.UtcNow,
                IsDeleted = false,

                CourierName = model.CourierName,
                ShippingPrice = shipping,
                Discount = discount,
                TotalPrice = grandTotal,

                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                AddressLine = model.AddressLine,
                City = model.City,
                PostalCode = model.PostalCode,
                Notes = model.Notes,
                PaymentMethod = model.PaymentMethod,
                ShippingStatus = "Processing",
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(3)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var item in cart.CartDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    PerfumeId = item.PerfumeId,
                    Quantity = item.Quantity,
                    MlPrice = item.Variant.Price,
                    VariantId = item.VariantId,
                    Ml = item.Variant.Ml
                };

                _db.OrderDetails.Add(orderDetail);
            }

            _db.CartDetails.RemoveRange(cart.CartDetails);
            await _db.SaveChangesAsync();

            return RedirectToAction("OrderSuccess", new { id = order.Id });
        }
        public async Task<IActionResult> OrderSuccess(int id)
        {
            var userId = _userManager.GetUserId(User);

            var order = await _db.Orders
                .Include(o => o.OrderDetail)
                    .ThenInclude(od => od.Perfume)
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return RedirectToAction("GetUserCart");

            return View(order);
        }
        private double GetCourierPrice(string courierName)
        {
            return courierName switch
            {
                "Econt" => 5.99,
                "Speedy" => 4.99,
                "BoxNow" => 3.49,
                _ => 4.99
            };
        }
        private double GetDiscountFromPromoCode(string promoCode, double subtotal)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
                return 0;

            promoCode = promoCode.Trim().ToUpper();

            return promoCode switch
            {
                "AURA10" => subtotal * 0.10,   // 10%
                "AURA20" => subtotal * 0.20,   // 20%
                "WELCOME5" => 5.00,            // 5 euro
                "FREE15" => 15.00,             // 15 euro
                _ => 0
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var cart = await _db.ShoppingCarts
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Perfume)
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Variant)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartDetails == null || !cart.CartDetails.Any())
                return RedirectToAction("GetUserCart");

            model.Cart = cart;
            model.Subtotal = cart.CartDetails.Sum(x => x.Variant.Price * x.Quantity);
            model.CourierName ??= "Speedy";
            model.ShippingPrice = GetCourierPrice(model.CourierName);
            model.Discount = GetDiscountFromPromoCode(model.PromoCode, model.Subtotal);
            model.GrandTotal = model.Subtotal + model.ShippingPrice - model.Discount;

            if (model.GrandTotal < 0)
                model.GrandTotal = 0;

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var orders = await _db.Orders
                .Include(o => o.OrderStatus)
                .Where(o => o.UserId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.CreateDate)
                .ToListAsync();

            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var order = await _db.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetail)
                    .ThenInclude(od => od.Perfume)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId && !o.IsDeleted);

            if (order == null)
                return RedirectToAction("MyOrders");

            return View(order);
        }
    }
}
