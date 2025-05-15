using PortfolioService.Application.DTOs;

namespace PortfolioService.Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<IEnumerable<StockAggregateDto>> GetStocks(string userId);
        Task<IsSuccessResultDto> AddStocks(string userId, string stockTicker, int quantity);
        Task<IsSuccessResultDto> RemoveStocks(string userId, string stockTicker, int quantityToSell);
    }
}