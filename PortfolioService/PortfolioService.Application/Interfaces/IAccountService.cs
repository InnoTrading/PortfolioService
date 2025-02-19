using PortfolioService.Application.DTOs;

namespace PortfolioService.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IsSuccessResultDto> CreateAccount(Guid userId);
        Task<IsSuccessResultDto> DeleteAccount(Guid userId);
        Task<IsSuccessResultDto> Deposit(Guid userId, decimal amount);
        Task<IsSuccessResultDto> Withdrawal(Guid userId, decimal amount);
        Task<BalanceDto> GetBalance(Guid userId);
        Task<ReservedBalanceDto> GetReservedBalance(Guid userId);
        Task<UserInfoDto> GetUserInfo(Guid userId);
    }
}