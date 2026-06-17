using AutoMapper;
using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.SessionViewModel;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Data.Models.Enums;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return false;
            if (model.StartDate <= DateTime.Now) return false;
            if(model.Capacity < 1 || model.Capacity > 25) return false;

            var trainer = await _unitOfWork.GetReposatory<Trainer>().GetByIdAsync(model.TrainerId);
            if(trainer == null) return false;

            var catagory = await _unitOfWork.GetReposatory<Catagory>().GetByIdAsync(model.CategoryId);
            if(catagory == null) return false;

            var isVaild = Enum.TryParse<Specialty>(catagory.CatagoryName, true,out var specialty); 
            if(!isVaild || trainer.Specialty != specialty)  return false;

            var session = _mapper.Map<CreateSessionViewModel,Session>(model);
            session.CatagoryId = model.CategoryId;
            _unitOfWork.GetReposatory<Session>().Add(session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.SessionReposatory.GetAllSessionsWithTrainerandCatagoryAsync(ct);
            if (result == null || !result.Any()) return null;
            var mappedSessions = _mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(result);
            foreach (var session in mappedSessions) 
            {
              session.AvailableSlots = session.Capacity -  await _unitOfWork.SessionReposatory.GetCountOfBookedSlotAsync(session.Id,ct);
            }
            return mappedSessions;
        }

        public async Task<IEnumerable<CatagorySelectViewModel>> GetCatagoriesForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetReposatory<Catagory>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CatagorySelectViewModel>>(result);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetReposatory<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }
    }
}
