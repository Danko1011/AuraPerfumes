using AuraPerfumes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PerfumesCount = await _db.Perfumes.CountAsync();
            ViewBag.OrdersCount = await _db.Orders.CountAsync(o => !o.IsDeleted);
            ViewBag.UsersCount = await _db.Users.CountAsync();
            ViewBag.TotalSales = await _db.Orders
                .Where(o => !o.IsDeleted)
                .SumAsync(o => (double?)o.TotalPrice) ?? 0;

            var latestOrders = await _db.Orders
                .Include(o => o.OrderStatus)
                .OrderByDescending(o => o.CreateDate)
                .Take(5)
                .ToListAsync();

            return View(latestOrders);
        }
    }
}
