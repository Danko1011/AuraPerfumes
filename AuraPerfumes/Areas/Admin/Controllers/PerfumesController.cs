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
        private readonly IWebHostEnvironment _env;

        public PerfumesController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var perfumes = await _db.Perfumes
                .Include(p => p.Gender)
                .Include(p => p.Variants)
                .ToListAsync();

            return View(perfumes);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Genders = new SelectList(await _db.Genders.ToListAsync(), "Id", "GenderLabel");
            return View(new Perfume());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Perfume perfume)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genders = new SelectList(await _db.Genders.ToListAsync(), "Id", "GenderLabel", perfume.GenderId);
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

            var genders = await _db.Genders.ToListAsync();

            ViewBag.DebugGenderCount = genders.Count;
            ViewBag.Genders = new SelectList(genders, "Id", "GenderLabel", perfume.GenderId);

            return View(perfume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Perfume perfume)
        {
            var existingPerfume = await _db.Perfumes.FindAsync(perfume.Id);
            if (existingPerfume == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Genders = new SelectList(await _db.Genders.ToListAsync(), "Id", "GenderLabel", perfume.GenderId);
                return View(perfume);
            }

            existingPerfume.PerfumeName = perfume.PerfumeName;
            existingPerfume.PerfumeModel = perfume.PerfumeModel;
            existingPerfume.Price = perfume.Price;
            existingPerfume.Description = perfume.Description;
            existingPerfume.GenderId = perfume.GenderId;
            existingPerfume.Image = perfume.Image;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var perfume = await _db.Perfumes
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (perfume == null)
                return NotFound();

            return View(perfume);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfume = await _db.Perfumes.FindAsync(id);
            if (perfume == null)
                return NotFound();

            _db.Perfumes.Remove(perfume);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}