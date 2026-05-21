using GymMangement.DbContexts;
using GymMangment.DAL.Reposatories.Classes;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GymMangement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanReposatory _planReposatory;
        public PlansController(PlanReposatory plan)
        {
            _planReposatory = plan;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plan = await _planReposatory.GetAllAsync(ct:ct);
            return View(plan);
        }
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planReposatory.GetByIdAsync(id, ct);
            if (plan == null)
                return RedirectToAction(nameof(Index));
            else
               return View(plan);
        }
    }
}
