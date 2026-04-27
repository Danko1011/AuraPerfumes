using AuraPerfumes.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _db.Orders
                .Include(o => o.OrderStatus)
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.CreateDate)
                .ToListAsync();

            ViewBag.Statuses = await _db.OrderStatuses.OrderBy(s => s.Status).ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetail)
                    .ThenInclude(od => od.Perfume)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

            if (order == null)
                return NotFound();

            ViewBag.Statuses = await _db.OrderStatuses.OrderBy(s => s.Status).ToListAsync();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int orderStatusId)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            var status = await _db.OrderStatuses.FindAsync(orderStatusId);

            if (status == null)
                return RedirectToAction(nameof(Index));

            order.OrderStatusId = orderStatusId;
            order.ShippingStatus = status.StatusName ?? order.ShippingStatus;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
