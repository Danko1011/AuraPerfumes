using AuraPerfumes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuraPerfumes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPerfumeImportController : Controller
    {
        private readonly PerfumeImportService _perfumeImportService;

        public AdminPerfumeImportController(PerfumeImportService perfumeImportService)
        {
            _perfumeImportService = perfumeImportService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["Error"] = "Моля качи CSV файл.";
                return RedirectToAction(nameof(Index));
            }

            if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Файлът трябва да е .csv.";
                return RedirectToAction(nameof(Index));
            }

            await using var stream = csvFile.OpenReadStream();
            var importedCount = await _perfumeImportService.ImportFromCsvAsync(stream);

            TempData["Success"] = $"Импортът приключи успешно. Добавени нови парфюми: {importedCount}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
