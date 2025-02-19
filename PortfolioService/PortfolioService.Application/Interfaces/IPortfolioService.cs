using PortfolioService.Application.DTOs;

namespace PortfolioService.Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<IEnumerable<StockAggregateDto>> GetStocks(string userId);
        Task<IsSuccessResultDto> AddStocks(string userId, Guid stockId, int quantity);
        Task<IsSuccessResultDto> RemoveStocks(string userId, Guid stockId, int quantityToSell);
    }
}