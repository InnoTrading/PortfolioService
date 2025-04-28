using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public record StockAggregateDto(string stockTicker, decimal totalQuantity);
}