using AuraPerfumes.Data;
using AuraPerfumes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PerfumeVariantsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PerfumeVariantsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int id)
        {
            var perfume = await _db.Perfumes
                .Include(p => p.Gender)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (perfume == null)
                return NotFound();

            return View(perfume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int perfumeId, int ml, double price)
        {
            if (ml <= 0 || price <= 0)
                return RedirectToAction(nameof(Index), new { id = perfumeId });

            var exists = await _db.PerfumeVariants.AnyAsync(v => v.PerfumeId == perfumeId && v.Ml == ml);

            if (!exists)
            {
                _db.PerfumeVariants.Add(new PerfumeVariant
                {
                    PerfumeId = perfumeId,
                    Ml = ml,
                    Price = price
                });

                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id = perfumeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var variant = await _db.PerfumeVariants.FindAsync(id);

            if (variant == null)
                return NotFound();

            var perfumeId = variant.PerfumeId;

            _db.PerfumeVariants.Remove(variant);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { id = perfumeId });
        }
    }
}
