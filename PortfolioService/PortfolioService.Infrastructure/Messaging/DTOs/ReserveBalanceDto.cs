namespace PortfolioService.Infrastructure.Messaging.Dtos
{
    public class ReserveBalanceRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
