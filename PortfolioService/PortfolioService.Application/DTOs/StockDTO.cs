using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public record StockDto(string StockTicker, int Quantity);
}
