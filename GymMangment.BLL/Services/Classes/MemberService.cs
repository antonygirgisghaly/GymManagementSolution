using AutoMapper;
using GymMangment.BLL.Services.AttachmentServices;
using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.MemberViewModels;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata;
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

        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetReposatory<Member>().GetAllAsync(ct: ct);
            var membermodels = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return membermodels;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Email == member.Email, ct);
            var Phone = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Phone == member.Phone, ct);
            if (members || Phone) return false;
            var stored = await _attachmentService.UploadAsync(member.PhotoFile.OpenReadStream(), member.PhotoFile.FileName, "MembersPhoto", ct);
            if(string.IsNullOrWhiteSpace(stored)) return false;
            var membermodel = _mapper.Map<Member>(member);
            membermodel.Photo = stored;
            _unitOfWork.GetReposatory<Member>().Add(membermodel);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if(result > 0) return true;
            else
            {
                _attachmentService.Delete(stored,"MembersPhoto");
                return false;
            }
        }
        public async Task<MemberViewModel?> GetMemberDetailsAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct);
            if (member == null) return null;
            var membermodel = _mapper.Map<Member, MemberViewModel>(member);
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
            var membermodel = _mapper.Map<HealthRecord, HealthRecordViewModel>(member);
            return membermodel;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct: ct);
            if (member == null) return null;
            var membermodel = _mapper.Map<Member, MemberToUpdateViewModel>(member);
            return membermodel;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel member, CancellationToken ct = default)
        {
            var memberUpdate = await _unitOfWork.GetReposatory<Member>().GetByIdAsync(id, ct);
            if(memberUpdate == null) return false;
            var phoneExists = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Phone == member.Phone && m.Id != id, ct);
            var emailExists = await _unitOfWork.GetReposatory<Member>().AnyAsync(m => m.Email == member.Email && m.Id != id, ct);
            if (phoneExists || emailExists) return false;
            _mapper.Map(member,memberUpdate);
            memberUpdate.UpdatedAt = DateTime.Now;
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
