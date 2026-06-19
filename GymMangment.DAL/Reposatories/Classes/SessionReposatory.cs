using GymMangment.DAL.Data.DbContexts;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Classes
{
    public class SessionReposatory : GenericReposatory<Session>, ISessionReposatory
    {
        private readonly GymDbContext _dbContext;
        public SessionReposatory(GymDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerandCatagoryAsync(CancellationToken ct = default)
        {
            var query = _dbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Catagory);
            return await query.ToListAsync(ct);
        }

        public async Task<int> GetCountOfBookedSlotAsync(int sessionid,CancellationToken ct = default)
        {
            return await _dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionid);
        }

        public async Task<Session?> GetSessionWithTrainerandCatagoryByIdAsync(int id, CancellationToken ct = default)
        {
            var query = await _dbContext.Sessions.AsNoTracking().Include(p => p.Trainer).Include(s => s.Catagory).FirstOrDefaultAsync(c => c.Id == id);
            return query;

        }
    }
}
