using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymMangement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct=default)
        {
            var result = await _sessionService.GetAllSessionsAsync(ct);
            return View(result); 
        }

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDownListAsync();
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid) 
            {
                await PopulateDownListAsync();
                return View(model);
            }
            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session_Created";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                await PopulateDownListAsync();
                return View(model);
            }
        }
        private async Task PopulateDownListAsync()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCatagoriesForDropDownAsync(), "Id", "CatagoryName");
        }
        #endregion
    }
}
