using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IAccountManager
    {
        Task<bool> CreateAccount(string userId);
        Task<bool> DeleteAccount(string userId);
        Task<bool> Deposit(string userId, decimal amount);
        Task<bool> Withdrawal(string userId, decimal amount);
        Task<decimal> Balance(string userId);
        Task<decimal> ReservedBalance(string userId);
        Task<bool> ReserveBalance(string userId, decimal amount);
        Task<bool> ReleaseReservedBalance(string userId, decimal amount);
    }
}