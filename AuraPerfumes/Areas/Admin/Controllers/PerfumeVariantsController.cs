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
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (perfume == null)
                return NotFound();

            ViewBag.PerfumeId = perfume.Id;
            ViewBag.PerfumeName = perfume.PerfumeName;
            ViewBag.PerfumeModel = perfume.PerfumeModel;

            return View(perfume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PerfumeVariant variant)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", new { id = variant.PerfumeId });

            _db.PerfumeVariants.Add(variant);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", new { id = variant.PerfumeId });
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

            return RedirectToAction("Index", new { id = perfumeId });
        }
    }
}