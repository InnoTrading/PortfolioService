namespace PortfolioService.Domain.Entities
{
    public class UserStockEntity : BaseEntity
    {
        public Guid UserID { get; set; }
        public virtual UserEntity Users { get; set; }
        public int Quantity { get; set; }
        public Guid StockID { get; set; }
        public virtual StockEntity Stock { get; set; }
    }
}
