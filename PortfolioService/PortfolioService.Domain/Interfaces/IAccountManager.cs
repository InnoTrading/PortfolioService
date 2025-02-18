using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IAccountManager
    {
        Task<bool> CreateAccount(Guid userID);
        Task<bool> DeleteAccount(Guid userID);
        Task<bool> Deposit(Guid userID, decimal amount);
        Task<bool> Withdraw(Guid userID, decimal amount);
        Task<decimal> Balance(Guid userID);
        Task<decimal> ReservedBalance(Guid userID);
        Task <UserEntity> GetUserInfo(Guid userID);
        Task <IEnumerable<UserStockEntity>> GetStocks(Guid userID);
        Task<bool> RemoveStock(Guid userID, Guid stockID, int quantityToSell);
        Task<bool> AddStocks(Guid userID, Guid stockID, int quantity);

    }
}
