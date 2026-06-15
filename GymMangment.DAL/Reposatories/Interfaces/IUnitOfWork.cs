using GymMangment.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenaricReposatory<TEntity> GetReposatory<TEntity>() where TEntity : BaseEntity ,new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        public ISessionReposatory SessionReposatory { get; }
    }
}
