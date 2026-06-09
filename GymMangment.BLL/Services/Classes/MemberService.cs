using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.MemberViewModels;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private IGenaricReposatory<Member> _memberService;
        private IGenaricReposatory<MemberShip> _memberShipService;
        private IGenaricReposatory<Plan> _planService;
        public MemberService(IGenaricReposatory<Member> memberService,IGenaricReposatory<MemberShip> memberShip,IGenaricReposatory<Plan> plan)
        {
            _memberService = memberService;
            _memberShipService = memberShip;
            _planService = plan;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberService.GetAllAsync(ct: ct);
            var membermodels = members.Select(m => new MemberViewModel()
            {
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString(),
                Name = m.Name,
                Id = m.Id
            });
            return membermodels;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var members = await _memberService.AnyAsync(m => m.Email == member.Email, ct);
            var Phone = await _memberService.AnyAsync(m => m.Phone == member.Phone, ct);
            if (members || Phone) return false;
            var membermodel = new Member()
            {
                Email = member.Email,
                Phone = member.Phone,
                Name = member.Name,
                Gender = member.Gender,
                DateOfBirth = member.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = member.BuildingNumber,
                    City = member.City,
                    Street = member.Street
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = member.HealthRecordViewModel.BloodType,
                    Weight = member.HealthRecordViewModel.Weight,
                    Height = member.HealthRecordViewModel.Height,
                    Note = member.HealthRecordViewModel.Note
                }
            };
            var result = await _memberService.AddAsync(membermodel);
            return result > 0;
        }
        public async Task<MemberViewModel?> GetMemberDetailsAsync(int id, CancellationToken ct = default)
        {
            var member = await _memberService.GetByIdAsync(id, ct);
            if (member == null) return null;
            var membermodel = new MemberViewModel()
            {
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                Name = member.Name,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber}, {member.Address.Street}, {member.Address.City}",
            };
            var activemembership = await _memberShipService.FirstOrDefaultAsync(m => m.MemberId == id && m.EndDate > DateTime.Now);
            if (activemembership is not null)
            {
            var plan = await _planService.GetByIdAsync(activemembership.PlanId);
                membermodel.PlanName = plan.Name;
                membermodel.StartDate = activemembership.CreatedAt.ToString();
                membermodel.EndDate = activemembership.EndDate.ToString();
            }
            return membermodel;
        }
    }
}
