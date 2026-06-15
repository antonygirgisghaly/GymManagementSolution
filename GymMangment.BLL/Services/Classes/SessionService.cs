using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.SessionViewModel;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.SessionReposatory.GetAllSessionsWithTrainerandCatagoryAsync(ct);
            if (result == null || !result.Any()) return null;
            var mappedSessions = result.Select(b => new SessionViewModel()
            {
                Id = b.Id,
                CategoryName = b.Catagory.CatagoryName,
                Capacity = b.Capacity,
                TrainerName = b.Trainer.Name,
                Description = b.Description,
                EndDate = b.EndDate,
                StartDate = b.StartDate,
            });
            foreach (var session in mappedSessions) 
            {
              session.AvailableSlots = session.Capacity -  await _unitOfWork.SessionReposatory.GetCountOfBookedSlotAsync(session.Id,ct);
            }
            return mappedSessions;
        }
    }
}
