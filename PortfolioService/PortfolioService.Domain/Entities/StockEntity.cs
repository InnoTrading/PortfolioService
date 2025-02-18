namespace PortfolioService.Domain.Entities
{
    public class StockEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
