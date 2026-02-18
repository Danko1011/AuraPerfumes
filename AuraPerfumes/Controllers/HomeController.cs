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

        public async Task<IActionResult> Index(string sterm = "", int genderId = 0)
        {
            
            IEnumerable<Perfume> perfumes = await _homeRepository.GetPerfumes(sterm, genderId);
            IEnumerable<Gender> genders = await _homeRepository.Genders();
            PerfumeDisplayModel perfumeModel = new PerfumeDisplayModel
            {
                Perfumes = perfumes,
                Genders=genders,
                STerm=sterm,
                GenderId=genderId
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
