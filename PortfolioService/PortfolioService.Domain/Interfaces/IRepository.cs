using PortfolioService.Domain.Entities;
using System.Linq.Expressions;

namespace PortfolioService.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<string> AddAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIDAsync(string id);
        Task<bool> UpdateAsync(T entity);
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);
    }
}
