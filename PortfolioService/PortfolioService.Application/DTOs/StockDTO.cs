using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public record StockDTO(StockEntity Stock, int Quantity);
}
