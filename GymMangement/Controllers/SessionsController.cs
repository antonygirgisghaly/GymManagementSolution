using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymMangement.PL.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Details([FromRoute]int id, CancellationToken ct) 
        {
            var result = await _sessionService.GetSessionByIdAsync(id,ct);
            if(result.success) 
            return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        { 
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if(result.success)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return RedirectToAction(nameof(Index));  
            }

        }
            [HttpPost]
        public async Task<IActionResult> Edit(int id,SessionToUpdateViewModel session, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return View(session);
            }
            var result = await _sessionService.UpdateSessionAsync(id,session,ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return View(session);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id,CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);
            if (result.success)
                return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id ,CancellationToken ct)
        {
            var result = await _sessionService.DeleteSessionAsync(id, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Deleted";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
