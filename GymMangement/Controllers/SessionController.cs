using GymMangment.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct=default)
        {
            var result = await _sessionService.GetAllSessionsAsync(ct);
            return View(result); 
        }
    }
}
