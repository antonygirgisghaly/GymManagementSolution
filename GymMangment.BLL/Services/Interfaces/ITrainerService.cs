using GymMangment.BLL.ViewModels.TrainerViewModel;
using GymMangment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);
        Task<bool> AddTrainerAsync(CreateTrainerViewModel trainer, CancellationToken ct = default);
        Task<TrainerViewModel?> GetTrainerByIdAsync(int id, CancellationToken ct = default);
        Task<EditTrainerViewModel> GetTrainerToUpdateAsync(int id, CancellationToken ct = default);
        Task<bool> UpdateTrainerAsync(int id, EditTrainerViewModel trainer, CancellationToken ct = default);
        Task<bool> DeleteTrainerAsync(int id, CancellationToken ct = default);
    }
}
