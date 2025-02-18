using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public record StockAggregateDTO(StockEntity Stock, decimal TotalQuantity);
}
