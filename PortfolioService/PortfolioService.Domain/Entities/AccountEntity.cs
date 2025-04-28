using PortfolioService.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

public class AccountEntity : BaseEntity
{
    public string Auth0UserId { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public decimal ReservedBalance { get; set; } = 0;
}
