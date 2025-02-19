using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IAccountManager
    {
        Task<bool> CreateAccount(Guid userId);
        Task<bool> DeleteAccount(Guid userId);
        Task<bool> Deposit(Guid userId, decimal amount);
        Task<bool> Withdrawal(Guid userId, decimal amount);
        Task<decimal> Balance(Guid userId);
        Task<decimal> ReservedBalance(Guid userId);
        Task <UserEntity> GetUserInfo(Guid userId);
    }
}
