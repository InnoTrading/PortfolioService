using PortfolioService.Domain.Entities;

namespace PortfolioService.Application.DTOs
{
    public class StockSummary
    {
        public StockEntity Stock { get; set; }
        public int TotalQuantity { get; set; }
    }
}
