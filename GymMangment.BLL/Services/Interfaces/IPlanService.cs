using GymMangment.BLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetPlanDetailsAsync(CancellationToken ct);
        Task<PlanViewModel?> GetByIdAsync(int id, CancellationToken ct);
        Task<EditPlanViewModel?> GetByIdEditPlanViewModelAsync(int id, CancellationToken ct);
        Task<bool> EditAsync(int id,EditPlanViewModel model, CancellationToken ct);
        Task<bool> Toogle(int id, CancellationToken ct);
    }
}
