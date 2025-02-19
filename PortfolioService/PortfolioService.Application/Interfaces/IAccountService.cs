using PortfolioService.Application.DTOs;

namespace PortfolioService.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IsSuccessResultDto> CreateAccount(RegisterDto userId);
        Task<IsSuccessResultDto> DeleteAccount(string userId);
        Task<IsSuccessResultDto> Deposit(string userId, decimal amount);
        Task<IsSuccessResultDto> Withdrawal(string userId, decimal amount);
        Task<BalanceDto> GetBalance(string userId);
        Task<ReservedBalanceDto> GetReservedBalance(string userId);
    }
}