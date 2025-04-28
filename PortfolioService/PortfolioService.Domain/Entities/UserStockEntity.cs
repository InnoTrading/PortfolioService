namespace PortfolioService.Domain.Entities
{
    public class UserStockEntity : BaseEntity
    {
        public string Auth0UserID { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
    }
}