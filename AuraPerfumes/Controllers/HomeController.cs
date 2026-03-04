using System.Diagnostics;
using AuraPerfumes.Models;
using AuraPerfumes.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuraPerfumes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger , IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string model = "", int genderId = 0, string designerName = "")
        {
            IEnumerable<Perfume> perfumes = await _homeRepository.GetPerfumes(model, genderId, designerName);
            IEnumerable<Gender> genders = await _homeRepository.Genders();
            IEnumerable<string> designers = await _homeRepository.Designers();

            PerfumeDisplayModel perfumeModel = new PerfumeDisplayModel
            {
                Perfumes = perfumes,
                Genders = genders,
                Designers = designers,
                Model = model,
                GenderId = genderId,
                DesignerName = designerName
            };

            return View(perfumeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
