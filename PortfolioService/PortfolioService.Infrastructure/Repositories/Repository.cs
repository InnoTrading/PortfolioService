using Microsoft.EntityFrameworkCore;
using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;
using System.Linq.Expressions;

namespace PortfolioService.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly PortfolioDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(PortfolioDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<string> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            var idProperty = typeof(T).GetProperty("ID");
            if (idProperty != null)
            {
                return (string)idProperty.GetValue(entity)!;
            }

            return string.Empty;
        }

        public Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIDAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}