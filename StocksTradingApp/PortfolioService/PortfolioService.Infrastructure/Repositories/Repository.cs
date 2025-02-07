using Microsoft.EntityFrameworkCore;
using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;
using System.Linq.Expressions;

namespace PortfolioService.Infrastructure.Repositories
{
    public class Repository<T>(PortfolioDbContext context) : IRepository<T> where T : Base
    {
        protected readonly PortfolioDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<string> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        
            var IDProperty = typeof(T).GetProperty("ID");
            if (IDProperty != null)
            {
                return (string)IDProperty.GetValue(entity)!;
            }

            return string.Empty;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIDAsync(string ID)
        {
            return await _dbSet.FindAsync(ID);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

    }
}
