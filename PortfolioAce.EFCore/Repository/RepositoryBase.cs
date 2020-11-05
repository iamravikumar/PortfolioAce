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

        public async Task<T> Delete(int id)
        {
            using (PortfolioAceDbContext context = _db.CreateDbContext())
            {
                T entity = await context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    return entity;
                }
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return entity;

            }
        }

        public async Task<T> GetById(int id)
        {
            using (PortfolioAceDbContext context = _db.CreateDbContext())
            {
                return await context.Set<T>().FindAsync(id);
            }   
        }

        public async Task<T> Update(T entity)
        {
            using (PortfolioAceDbContext context = _db.CreateDbContext())
            {
                context.Set<T>().Update(entity);

                await context.SaveChangesAsync();

                return entity;

            }
        }
    }
}
