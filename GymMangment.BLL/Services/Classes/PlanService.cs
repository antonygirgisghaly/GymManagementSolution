using GymMangment.BLL.Services.Interfaces;
using GymMangment.BLL.ViewModels.PlanViewModel;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Classes;
using GymMangment.DAL.Reposatories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GymMangment.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfOfWork;
        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfOfWork = unitOfWork;
        }


        public async Task<PlanViewModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetByIdAsync(id, ct);
            if (result == null) return null;
            var plan = new PlanViewModel
            {
                Id = result.Id,
                Name = result.Name,
                IsActive = result.IsActive,
                Description = result.Description,
                DuirationDays = result.DuirationDays,
                Price = result.Price
            };
            return plan;
        }

        public async Task<EditPlanViewModel?> GetByIdEditPlanViewModelAsync(int id, CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetByIdAsync(id, ct);
            if (result == null) return null;
            var plan = new EditPlanViewModel
            {
                PlanName = result.Name,
                DurationDays = result.DuirationDays,
                Description = result.Description,
                Price = result.Price,
            };
            return plan;
        }

        public async Task<IEnumerable<PlanViewModel>> GetPlanDetailsAsync(CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetAllAsync(ct: ct);
            var plan = result.Select( p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = p.IsActive,
                Description = p.Description,
                DuirationDays = p.DuirationDays,
                Price = p.Price
            });
            return plan;
        }
        public async Task<bool> EditAsync(int id, EditPlanViewModel model, CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetByIdAsync(id, ct);
            if (result == null || !result.IsActive) return false;
            var activememberships = await _unitOfOfWork.GetReposatory<MemberShip>().AnyAsync(p => p.Id == id && p.EndDate > DateTime.Now, ct);
            if (activememberships) return false;
            else
            {
                result.Name = model.PlanName;
                result.DuirationDays = model.DurationDays;
                result.Price = model.Price;
                result.Description = model.Description;
                _unitOfOfWork.GetReposatory<Plan>().Update(result);
                var update = await _unitOfOfWork.SaveChangesAsync(ct);
                return update > 0;
            }
        }

        public async Task<bool> Toogle(int id, CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetByIdAsync(id, ct);
            if (result == null) return false;
            result.IsActive = !result.IsActive;
            _unitOfOfWork.GetReposatory<Plan>().Update(result);
            var update = await _unitOfOfWork.SaveChangesAsync(ct);
            return update > 0;
        }
    }
}
