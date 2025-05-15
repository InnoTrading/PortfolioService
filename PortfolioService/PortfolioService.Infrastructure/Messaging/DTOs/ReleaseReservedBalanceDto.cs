namespace PortfolioService.Infrastructure.Messaging.Dtos
{
    public class ReleaseReservedBalanceRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
