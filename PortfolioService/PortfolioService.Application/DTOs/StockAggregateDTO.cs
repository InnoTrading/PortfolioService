using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public record StockAggregateDto(StockEntity Stock, decimal TotalQuantity);
}