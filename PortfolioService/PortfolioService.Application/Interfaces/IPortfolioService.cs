using PortfolioService.Application.DTOs;

namespace PortfolioService.Application.Interfaces
{
    public interface IPortfolioService
    {
        Task<IEnumerable<StockAggregateDto>> GetStocks(Guid userId);
        Task<IsSuccessResultDto> AddStocks(Guid userId, Guid stockId, int quantity);
        Task<IsSuccessResultDto> RemoveStocks(Guid userId, Guid stockId, int quantityToSell);
    }
}