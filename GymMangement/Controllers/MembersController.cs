using GymMangment.BLL.Services.Interfaces;
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
        //Get  BaseUrl/HealthRecords/Details/{id}
        //Details of specific member health record

        #region Create
        //Get  BaseUrl/Members/Create/{id}
        //Form to create new member

        //Post BaseUrl/Members/Create   {Member}
        //Handle form data to create new member
        #endregion

        #region Edit
        //Get  BaseUrl/Members/Edit/{id}
        //Form to edit existing member

        //Post BaseUrl/Members/Edit   {Member}
        //Handle form data to edit existing member
        #endregion

        #region Delete
        //Get  BaseUrl/Members/Delete/{id}
        //Form to delete existing member

        //Post BaseUrl/Members/Delete   {Member}
        //Handle form data to delete existing member
        #endregion

    }
}
