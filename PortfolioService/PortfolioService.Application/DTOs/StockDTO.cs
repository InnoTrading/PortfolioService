using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public record StockDto(StockEntity Stock, int Quantity);
}
