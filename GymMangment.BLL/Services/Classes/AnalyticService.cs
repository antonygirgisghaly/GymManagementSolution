using AutoMapper.Execution;
using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.AnalyticViewModels;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Classes
{
    public class AnalyticService : IAnaltyicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalyticViewModel> GetDataAsync(CancellationToken ct)
        {
            var now = DateTime.Now;
            var upcomingSessions = await _unitOfWork.GetReposatory<Session>().CountAsync(x => x.StartDate > now, ct);
            var ongoingSessions = await _unitOfWork.GetReposatory<Session>().CountAsync(s =>  s.StartDate <= now && s.EndDate >= now, ct);
            var completedSessions = await _unitOfWork.GetReposatory<Session>().CountAsync(x => x.EndDate < now, ct);
            var totalMembers = await _unitOfWork.GetReposatory<DAL.Data.Models.Member>().CountAsync(ct: ct);
            var activeMembers = await _unitOfWork.GetReposatory<MemberShip>().CountAsync(x => x.EndDate > now,ct);
            var totalTrainers = await _unitOfWork.GetReposatory<Trainer>().CountAsync(ct: ct);

            return new AnalyticViewModel
            {
                TotlaMembers = totalMembers,
                UpcomingSessions = upcomingSessions,
                OngoingSessions = ongoingSessions,
                CompletedSessions = completedSessions,
                ActiveMembers = activeMembers,
                Trainers = totalTrainers
            };
        }
    }
}
