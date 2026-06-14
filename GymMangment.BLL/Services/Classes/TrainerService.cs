using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.TrainerViewModel;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetReposatory<Trainer>().GetAllAsync(ct : ct);
            var trainerViewModels = trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialty.ToString(),
                Details = $"Details for {t.Name}",
                Edit = $"Edit {t.Name}",
                Delete = $"Delete {t.Name}"
            });
            return trainerViewModels;
        }
        public async Task<bool> AddTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetReposatory<Trainer>().AnyAsync(m => m.Email == trainer.Email, ct);
            var Phone = await _unitOfWork.GetReposatory<Trainer>().AnyAsync(m => m.Phone == trainer.Phone, ct);
            if (members || Phone) return false;
            var trainerViewModel = new Trainer
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialties,
                Address = new Address()
                {
                    BuildingNumber = trainer.BuildingNumber,
                    City = trainer.City,
                    Street = trainer.Street
                },
            };
            _unitOfWork.GetReposatory<Trainer>().Add(trainerViewModel);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetReposatory<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return null;
            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialty.ToString(),
                DateOfBirth = trainer.DateOfBirth,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street
            };
        }

        public async Task<EditTrainerViewModel> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetReposatory<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return null;
            var editTrainerViewModel = new EditTrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialty,
                DateOfBirth = trainer.DateOfBirth,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street
            };
            return editTrainerViewModel;
        }

        public async Task<bool> UpdateTrainerAsync(int id, EditTrainerViewModel trainer, CancellationToken ct = default)
        {
            var phone = await _unitOfWork.GetReposatory<Trainer>().AnyAsync(m => m.Phone == trainer.Phone && m.Id != id, ct);
            var email = await _unitOfWork.GetReposatory<Trainer>().AnyAsync(m => m.Email == trainer.Email && m.Id != id, ct);
            if (phone|| email) return false;
            var trainerToUpdate = new Trainer
            {
                Id = id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialty,
                DateOfBirth = trainer.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = trainer.BuildingNumber,
                    City = trainer.City,
                    Street = trainer.Street
                }
            };
            _unitOfWork.GetReposatory<Trainer>().Update(trainerToUpdate);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetReposatory<Trainer>().GetByIdAsync(id,ct);
            if (result == null) return false;
            var trainer = await _unitOfWork.GetReposatory<Session>().AnyAsync(b => b.TrainerId == id && b.StartDate > DateTime.Now,ct);
            if(trainer) return false;
            _unitOfWork.GetReposatory<Trainer>().Delete(result);
            var final = await _unitOfWork.SaveChangesAsync();
            return final > 0;
        }
    }
}
