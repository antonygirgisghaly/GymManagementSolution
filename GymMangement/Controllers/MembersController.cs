using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.MemberViewModels;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMangement.PL.Controllers
{
    public class MembersController : Controller
    {
        private IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        //Get  BaseUrl/Members/Index
        //List of all members
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        //Get  BaseUrl/Members/Details/{id}
        //Details of specific member
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var memberDetails = await _memberService.GetMemberDetailsAsync(id, ct);
            if (memberDetails == null)
            { 
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index)); 
            }
            return View(memberDetails);
        }
        //Get  BaseUrl/HealthRecords/Details/{id}
        //Details of specific member health record
        public async Task<IActionResult> HealthRecordDetails(int id ,CancellationToken ct)
         {
            var healthRecordDetails = await _memberService.GetHealthRecordDetailsAsync(id, ct);
            if (healthRecordDetails == null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecordDetails);
        }
        #region Create
        //Get  BaseUrl/Members/Create/{id}
        //Form to create new member
        [HttpGet]
        public IActionResult Create() => View();

        //Post BaseUrl/Members/Create   {Member}
        //Handle form data to create new member
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel member, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), member);
            }
            var createdMember = await _memberService.CreateMemberAsync(member, ct);
            if (createdMember)
                TempData["SuccessMessage"] = "Member created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create member. Please try again.";

                return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        //Get  BaseUrl/Members/Edit/{id}
        //Form to edit existing member
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var memberToEdit = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (memberToEdit == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(memberToEdit);
        }
        //Post BaseUrl/Members/Edit   {Member}
        //Handle form data to edit existing member
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute]int id, MemberToUpdateViewModel member, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(member);
            var result = await _memberService.UpdateMemberDetailsAsync(id, member, ct);
            if (result)
                TempData["SuccessMessage"] = "Member updated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to update member. Please try again.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        //Get  BaseUrl/Members/Delete/{id}
        //Form to delete existing member
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if(member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        //Post BaseUrl/Members/Delete   {Member}
        //Handle form data to delete existing member
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);
            if(result)
                TempData["SuccessMessage"] = "Member deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete member. Please try again.";
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
