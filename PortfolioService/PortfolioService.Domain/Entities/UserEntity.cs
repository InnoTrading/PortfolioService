namespace PortfolioService.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}