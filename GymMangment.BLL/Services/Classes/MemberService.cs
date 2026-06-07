using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.MemberViewModels;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.Build.Framework;
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

        public MemberService(IGenaricReposatory<Member> memberService) 
        {
            _memberService = memberService;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberService.GetAllAsync(ct : ct);
            if (members.Any()) return [] ;
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
    }
}
