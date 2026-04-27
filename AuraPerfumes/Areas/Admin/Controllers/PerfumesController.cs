using AuraPerfumes.Data;
using AuraPerfumes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuraPerfumes.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PerfumesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PerfumesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _db.Perfumes
                .Include(p => p.Gender)
                .Include(p => p.Variants)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.PerfumeName.Contains(search) ||
                    p.PerfumeModel.Contains(search));
            }

            ViewBag.Search = search;

            var perfumes = await query
                .OrderBy(p => p.PerfumeName)
                .ThenBy(p => p.PerfumeModel)
                .ToListAsync();

            return View(perfumes);
        }

        public async Task<IActionResult> Create()
        {
            await LoadGenders();
            return View(new Perfume());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Perfume perfume)
        {
            if (!ModelState.IsValid)
            {
                await LoadGenders();
                return View(perfume);
            }

            _db.Perfumes.Add(perfume);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var perfume = await _db.Perfumes.FindAsync(id);

            if (perfume == null)
                return NotFound();

            await LoadGenders();
            return View(perfume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Perfume perfume)
        {
            if (id != perfume.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadGenders();
                return View(perfume);
            }

            var existing = await _db.Perfumes.FindAsync(id);

            if (existing == null)
                return NotFound();

            existing.PerfumeName = perfume.PerfumeName;
            existing.PerfumeModel = perfume.PerfumeModel;
            existing.Price = perfume.Price;
            existing.Image = perfume.Image;
            existing.GenderId = perfume.GenderId;
            existing.Description = perfume.Description;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var perfume = await _db.Perfumes
                .Include(p => p.Gender)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (perfume == null)
                return NotFound();

            return View(perfume);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfume = await _db.Perfumes
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (perfume == null)
                return NotFound();

            if (perfume.Variants != null && perfume.Variants.Any())
                _db.PerfumeVariants.RemoveRange(perfume.Variants);

            _db.Perfumes.Remove(perfume);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadGenders()
        {
            ViewBag.Genders = new SelectList(await _db.Genders.OrderBy(g => g.GenderLabel).ToListAsync(), "Id", "GenderLabel");
        }
    }
}
