using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IPortfolioManager
    {
        Task<IEnumerable<UserStockEntity>> GetStocks(Guid userId);
        Task<bool> RemoveStock(Guid userId, Guid stockId, int quantityToSell);
        Task<bool> AddStocks(Guid userId, Guid stockId, int quantity);
    }
}   
