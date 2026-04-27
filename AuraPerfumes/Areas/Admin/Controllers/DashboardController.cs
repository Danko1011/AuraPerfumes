using AuraPerfumes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PerfumesCount = await _db.Perfumes.CountAsync();
            ViewBag.OrdersCount = await _db.Orders.CountAsync(o => !o.IsDeleted);
            ViewBag.UsersCount = _userManager.Users.Count();
            ViewBag.TotalSales = await _db.Orders.Where(o => !o.IsDeleted).SumAsync(o => (double?)o.TotalPrice) ?? 0;

            var latestOrders = await _db.Orders
                .Include(o => o.OrderStatus)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.CreateDate)
                .Take(6)
                .ToListAsync();

            return View(latestOrders);
        }
    }
}
