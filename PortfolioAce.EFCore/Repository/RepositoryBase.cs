using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioAce.EFCore.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly PortfolioAceDbContextFactory _db; 

        public RepositoryBase(PortfolioAceDbContextFactory db)
        {
            this._db = db;
        }

        public async Task<T> Create(T entity)
        {
            using (PortfolioAceDbContext context = _db.CreateDbContext())
            {
                EntityEntry<T> res = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return res.Entity;

            }
        }

        public async Task<T> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetByCondition(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
