using GymMangment.DAL.Data.DbContexts;
using GymMangment.DAL.Data.Models;
using GymMangment.DAL.Reposatories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangment.DAL.Reposatories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];
        public ISessionReposatory SessionReposatory { get; }
        public UnitOfWork(GymDbContext dbContext,ISessionReposatory session) 
        {
            _dbContext = dbContext;
            SessionReposatory = session;
        }


        public IGenaricReposatory<TEntity> GetReposatory<TEntity>() where TEntity : BaseEntity, new()
        {
            var typename = typeof(TEntity).Name;
            if(_repositories.TryGetValue(typename,out object? value))
            {
                return (IGenaricReposatory<TEntity>)value;
            }
            else
            {
                var result = new GenericReposatory<TEntity>(_dbContext);
                _repositories[typename] = result;
                return result;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _dbContext.SaveChangesAsync(ct);
    }
}
