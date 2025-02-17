using PortfolioService.Application.DTOs;

namespace PortfolioService.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IsSuccessResultDto> CreateAccount(Guid userID);
        Task<IsSuccessResultDto> DeleteAccount(Guid userID);
        Task<IsSuccessResultDto> Deposit(Guid userID, decimal amount);
        Task<IsSuccessResultDto> Withdraw(Guid userID, decimal amount);
        Task<BalanceDto> GetBalance(Guid userID);
        Task<ReservedBalanceDto> GetReservedBalance(Guid userID);
        Task<UserInfoDto> GetUserInfo(Guid userID);
        Task<IEnumerable<StockAggregateDTO>> GetStocks(Guid userID);
    }

}
