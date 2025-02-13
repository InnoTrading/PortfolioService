namespace PortfolioService.Domain.Entities
{
    public class AccountEntity : BaseEntity
    {
        public Guid UserID { get; set; }
        public virtual UserEntity User { get; set; }
        public decimal Balance { get; set; }
        public decimal ReservedBalance { get; set; }
    }
}

