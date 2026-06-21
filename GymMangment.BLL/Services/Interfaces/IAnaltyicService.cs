using GymMangment.BLL.ViewModels.AnalyticViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.BLL.Services.Interfaces
{
    public interface IAnaltyicService
    {
        Task<AnalyticViewModel> GetDataAsync(CancellationToken ct);
    }
}
