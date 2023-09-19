using Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T, TContext> : IRepositoryBase<T> where T : class where TContext : DbContext
    {
        protected TContext _dbContext;
        public RepositoryBase(TContext repositoryContext)
        {
            _dbContext = repositoryContext;
        }
        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? _dbContext.Set<T>()
                .AsNoTracking() : _dbContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ? _dbContext.Set<T>()
            .Where(expression).AsNoTracking() : _dbContext.Set<T>().Where(expression);
        }

        public void Create(T entity) => _dbContext.Set<T>().Add(entity);
        public void Update(T entity) => _dbContext.Set<T>().Update(entity);
        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);
    }
}