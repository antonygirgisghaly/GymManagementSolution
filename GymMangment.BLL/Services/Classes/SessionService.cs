using AutoMapper;
using GymMangment.BLL.Comman;
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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("Start date must be before end date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start date must be in the future");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 and 25");
            ;

            var trainer = await _unitOfWork.GetReposatory<Trainer>().GetByIdAsync(model.TrainerId);
            if(trainer == null) return Result.NotFound("Trainer not found");

            var catagory = await _unitOfWork.GetReposatory<Catagory>().GetByIdAsync(model.CategoryId);
            if(catagory == null) return Result.NotFound("Capacity not found");

            var isVaild = Enum.TryParse<Specialty>(catagory.CatagoryName, true,out var specialty); 
            if(!isVaild || trainer.Specialty != specialty)  return Result.Validation("Can not create this session for Specialty not found or trainer with not the same specialty");

            var session = _mapper.Map<CreateSessionViewModel,Session>(model);
            session.CatagoryId = model.CategoryId;
            _unitOfWork.GetReposatory<Session>().Add(session);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.Ok() : Result.NotFound("Failed to create session");
        }

public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
{
                var session = await _unitOfWork.SessionReposatory.GetByIdAsync(id, ct);
                if (session == null) return Result.NotFound("Session not found");
            
                if (session.StartDate <= DateTime.Now) return Result.Fail("Can not delete session started");
            
                var bookingCount = await _unitOfWork.SessionReposatory.GetCountOfBookedSlotAsync(id, ct);
                if (bookingCount > 0) return Result.Fail("Can not delete session that already has bookings");
            
                _unitOfWork.SessionReposatory.Delete(session);
                var result = await _unitOfWork.SaveChangesAsync(ct);
                return result > 0 ? Result.Ok() : Result.Fail("Failed to delete session");
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

        public async Task<Result<SessionViewModel?>> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionReposatory.GetSessionWithTrainerandCatagoryByIdAsync(id,ct);
            if (session == null) return Result<SessionViewModel?>.NotFound("Session not found");
            var result = _mapper.Map<Session,SessionViewModel>(session);
            result.AvailableSlots = result.Capacity - await _unitOfWork.SessionReposatory.GetCountOfBookedSlotAsync(id, ct);
            return Result<SessionViewModel?>.Ok(result);
        }

        public async Task<Result<SessionToUpdateViewModel>> GetSessionToUpdateAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionReposatory.GetByIdAsync(id, ct);
            if (session == null) return Result<SessionToUpdateViewModel>.NotFound("Session not found");
            if (session.StartDate <= DateTime.Now) return Result<SessionToUpdateViewModel>.Fail("Can not update session stared");
            var bookingCount = await _unitOfWork.SessionReposatory.GetCountOfBookedSlotAsync(id,ct);
            if (bookingCount > 0) return Result<SessionToUpdateViewModel>.Fail("Can not Update Session that Already has bookings");
            var map = _mapper.Map<Session,SessionToUpdateViewModel>(session);
            return Result<SessionToUpdateViewModel>.Ok(map);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetReposatory<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result> UpdateSessionAsync(int id, SessionToUpdateViewModel session, CancellationToken ct = default)
        {
            var update = await _unitOfWork.SessionReposatory.GetByIdAsync(id, ct);
            if (update == null) return Result.NotFound("Session not found");

            if (update.StartDate <= DateTime.Now) return Result.Fail("Can not update session stared");

            if (session.EndDate <= DateTime.Now) return Result.Fail("End Date can not be after start date");

            var bookingCount = await _unitOfWork.SessionReposatory.GetCountOfBookedSlotAsync(id, ct);
            if (bookingCount > 0) return Result.Fail("Can not Update Session that Already has bookings");

            if (session.StartDate <= DateTime.Now) return Result.Fail("Start date must be in the future");

            var trainer = await _unitOfWork.GetReposatory<Trainer>().GetByIdAsync(update.TrainerId);
            if(trainer == null) return Result.NotFound("Trainer not found");

            var catagory = await _unitOfWork.GetReposatory<Catagory>().GetByIdAsync(update.CatagoryId);

            var isVaild = Enum.TryParse<Specialty>(catagory?.CatagoryName, true, out var specialty);
            if (!isVaild || trainer.Specialty != specialty) return Result.Validation("Can not create this session to this trainer");

            _mapper.Map(session, update);
            update.UpdatedAt = DateTime.Now;
            _unitOfWork.SessionReposatory.Update(update);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Session fail to create");

        }
    }
}
