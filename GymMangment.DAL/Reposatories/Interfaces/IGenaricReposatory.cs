using GymMangment.DAL.Data.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Interfaces
{
    public interface IGenaricReposatory<TEntity>  where TEntity : BaseEntity , new()
    {
        public Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        public Task<int> AddAsync(TEntity entity);
        public Task<int> UpdateAsync(TEntity entity);
        public Task<int> DeleteAsync(TEntity entity);
    }
}
