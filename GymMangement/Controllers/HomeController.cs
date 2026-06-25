using GymMangement.Models;
using GymMangment.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymMangement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnaltyicService _analyticService;

        public HomeController(ILogger<HomeController> logger, IAnaltyicService analyticService)
        {
            _logger = logger;
            _analyticService = analyticService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _analyticService.GetDataAsync(ct);
            return View(result);
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
