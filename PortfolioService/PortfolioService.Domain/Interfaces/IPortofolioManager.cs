using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IPortfolioManager
    {
        Task<IEnumerable<UserStockEntity>> GetStocks(string userId);
        Task<bool> RemoveStocks(string userId, Guid stockId, int quantityToSell);
        Task<bool> AddStocks(string userId, Guid stockId, int quantity);
    }
}