using GymMangment.BLL.Services.Classes;
using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.TrainerViewModel;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    public class TrainersController : Controller
    {
        private ITrainerService _trainerService;
        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }
        #region Create
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel trainer, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data";
                return View(trainer);
            }
            var result = await _trainerService.AddTrainerAsync(trainer, ct);
            if (result == false)
            {
                TempData["ErrorMessage"] = "Failed to add trainer";
                return View(trainer);
            }
            TempData["SuccessMessage"] = "Trainer added successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id, CancellationToken ct = default) 
        {
            var trainer = await _trainerService.GetTrainerByIdAsync(id, ct);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken ct) 
        {
            var result = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (result == null) 
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, EditTrainerViewModel trainer, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data";
                return View(trainer);
            }
            // Implementation for updating trainer
            var result = await _trainerService.UpdateTrainerAsync(id, trainer, ct);
            TempData["SuccessMessage"] = "Trainer updated successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _trainerService.GetTrainerByIdAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _trainerService.DeleteTrainerAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete trainer. Please try again.";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
