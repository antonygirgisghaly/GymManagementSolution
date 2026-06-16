using AutoMapper;
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
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<PlanViewModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetByIdAsync(id, ct);
            if (result == null) return null;
            var plan = _mapper.Map<Plan, PlanViewModel>(result);
            return plan;
        }

        public async Task<EditPlanViewModel?> GetByIdEditPlanViewModelAsync(int id, CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetByIdAsync(id, ct);
            if (result == null) return null;
            var plan = _mapper.Map<Plan, EditPlanViewModel>(result);
            return plan;
        }

        public async Task<IEnumerable<PlanViewModel>> GetPlanDetailsAsync(CancellationToken ct)
        {
            var result = await _unitOfOfWork.GetReposatory<Plan>().GetAllAsync(ct: ct);
            var plan = _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(result);
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
                _mapper.Map(model,result);
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
