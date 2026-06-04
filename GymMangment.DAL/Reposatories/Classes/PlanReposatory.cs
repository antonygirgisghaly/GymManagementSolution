using GymMangement.DbContexts;
using GymMangement.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Classes
{
    public class PlanReposatory : IPlanReposatory
    {
        private readonly GymDbContext dbContext;
        public PlanReposatory(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
           dbContext.Plan.Add(plan);
           return await dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plan.Remove(plan);
            return await dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query =tracking ? dbContext.Plan : dbContext.Plan.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await dbContext.Plan.FindAsync(id, ct);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            dbContext.Plan.Update(plan);
            return await dbContext.SaveChangesAsync(ct);
        }
    }
}
