using PortfolioService.Application.DTOs;
using PortfolioService.Application.Interfaces;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Application.Services
{
    public class PortfolioService(IPortfolioManager portfolioManager) : IPortfolioService
    {
        private readonly IPortfolioManager _portfolioManager = portfolioManager;

        public async Task<IEnumerable<StockAggregateDto>> GetStocks(string userId)
        {
            var userStocks = await _portfolioManager.GetStocks(userId);

            var stockSummaries = userStocks.Select(s =>
                new StockAggregateDto(s.StockTicker, s.Quantity)
            );

            return stockSummaries;
        }

        public async Task<IsSuccessResultDto> AddStocks(string userId, string stockTicker, int quantity)
        {
            var result = await _portfolioManager.AddStocks(userId, stockTicker, quantity);
            return new IsSuccessResultDto(result);
        }

        public async Task<IsSuccessResultDto> RemoveStocks(string userId, string stockTicker, int quantityToSell)
        {
            var result = await _portfolioManager.RemoveStocks(userId, stockTicker, quantityToSell);
            return new IsSuccessResultDto(result);
        }
    }
}