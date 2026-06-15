using GymMangment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Interfaces
{
    public interface ISessionReposatory : IGenaricReposatory<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerandCatagoryAsync(CancellationToken ct = default);
        Task<int> GetCountOfBookedSlotAsync(int id,CancellationToken ct = default);
    }
}
