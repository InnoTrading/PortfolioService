using PortfolioService.Application.DTOs;
using PortfolioService.Application.Interfaces;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Application.Services
{
    public class PortfolioService(IPortfolioManager portfolioManager) : IPortfolioService
    {
        private readonly IPortfolioManager _portfolioManager = portfolioManager;

        public async Task<IEnumerable<StockAggregateDTO>> GetStocks(Guid userId)
        {
            var userStocks = await _portfolioManager.GetStocks(userId);

            var stockSummaries = userStocks.Select(s =>
                new StockAggregateDTO(s.Stock, s.Quantity)
            );

            return stockSummaries;
        }

        public async Task<IsSuccessResultDto> AddStocks(Guid userId, Guid stockId, int quantity)
        {
            var result = await _portfolioManager.AddStocks(userId, stockId, quantity);
            return new IsSuccessResultDto(result);
        }

        public async Task<IsSuccessResultDto> RemoveStocks(Guid userId, Guid stockId, int quantityToSell)
        {
            var result = await _portfolioManager.RemoveStock(userId, stockId, quantityToSell);
            return new IsSuccessResultDto(result);
        }
    }
}