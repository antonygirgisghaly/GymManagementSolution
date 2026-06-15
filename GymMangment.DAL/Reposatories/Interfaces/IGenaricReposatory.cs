using GymMangment.DAL.Data.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Interfaces
{
    public interface IGenaricReposatory<TEntity>  where TEntity : BaseEntity , new()
    {
        public Task<TEntity> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);
        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,bool tracking = false, CancellationToken ct = default);
    }
}
