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
        private readonly IUnitOfWork _unitOfWork;
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetReposatory<Member>().GetAllAsync(ct: ct);
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
            var members = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Email == member.Email, ct);
            var Phone = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Phone == member.Phone, ct);
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
            _unitOfWork.GetReposatory<Member>().Add(membermodel);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<MemberViewModel?> GetMemberDetailsAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct);
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
            var activemembership = await _unitOfWork.GetReposatory<MemberShip>().FirstOrDefaultAsync(m => m.MemberId == id && m.EndDate > DateTime.Now);
            if (activemembership is not null)
            {
            var plan = await _unitOfWork.GetReposatory<Plan>().GetByIdAsync(activemembership.PlanId);
                membermodel.PlanName = plan.Name;
                membermodel.StartDate = activemembership.CreatedAt.ToString();
                membermodel.EndDate = activemembership.EndDate.ToString();
            }
            return membermodel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetReposatory<HealthRecord>().FirstOrDefaultAsync(m => m.Id == id, ct:ct);
            if (member == null) return null;
            var membermodel = new HealthRecordViewModel() 
            {
                BloodType = member.BloodType,
                Weight = member.Weight,
                Height = member.Height,
                Note = member.Note
            };
            return membermodel;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct: ct);
            if (member == null) return null;
            var membermodel = new MemberToUpdateViewModel()
            {
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Name = member.Name,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street
            };
            return membermodel;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel member, CancellationToken ct = default)
        {
            var memberUpdate = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct);
            if(memberUpdate == null) return false;
            var phoneExists = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Phone == member.Phone && m.Id != id, ct);
            var emailExists = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Email == member.Email && m.Id != id, ct);
            if (phoneExists || emailExists) return false;
            memberUpdate.Email = member.Email;
            memberUpdate.Phone = member.Phone;
            memberUpdate.Address.BuildingNumber = member.BuildingNumber;
            memberUpdate.Address.City = member.City;
            memberUpdate.Address.Street = member.Street;
            _unitOfWork.GetReposatory<Member>().Update(memberUpdate);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default) 
        {
            var result = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct);
            if (result == null) return false;
            var checkbooking = await _unitOfWork.GetReposatory<Booking>().AnyAsync(b => b.MemberId == id && b.Session.StartDate > DateTime.Now, ct);
            if (checkbooking) return false;
            _unitOfWork.GetReposatory<Member>().Delete(result);
            var deleteResult = await _unitOfWork.SaveChangesAsync(ct);
            return deleteResult > 0;
        }
    }
}
