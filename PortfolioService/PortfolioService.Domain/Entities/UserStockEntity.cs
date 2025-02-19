namespace PortfolioService.Domain.Entities
{
    public class UserStockEntity : BaseEntity
    {
        public string Auth0UserID { get; set; }
        public Guid StockID { get; set; }
        public int Quantity { get; set; }
        public virtual StockEntity Stock { get; set; }
    }
}