using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.PlanViewModel;
using GymMangment.DAL.Data.DbContexts;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Classes;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GymMangment.PL.Controllers
{
    [Authorize]
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;
        public PlansController(IPlanService plan)
        {
            _planService = plan;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plan = await _planService.GetPlanDetailsAsync(ct: ct);
            return View(plan);
        }
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetByIdAsync(id, ct);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            else
                TempData["SuccessMessage"] = "Plan details retrieved successfully.";
            return View(plan);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _planService.GetByIdEditPlanViewModelAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            else
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, EditPlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid plan data.";
                return View(model);
            }
            var result = await _planService.EditAsync(id, model, ct);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to edit the plan.";
                return View(model);
            }
            else
                TempData["SuccessMessage"] = "Plan edited successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id, CancellationToken ct)
        {
            var result = await _planService.Toogle(id, ct);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to toggle plan status.";
                return RedirectToAction(nameof(Index));
            }
            else
                TempData["SuccessMessage"] = "Plan status toggled successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}