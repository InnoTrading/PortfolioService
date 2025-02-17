using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Application.Services
{
    public class AccountManager(IUnitOfWork unitOfWork) : IAccountManager
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private async Task<AccountEntity> GetAccountByUserID(Guid userID)
        {
            var account = (await _unitOfWork.Accounts.GetByConditionAsync(a => a.UserID == userID)).SingleOrDefault();

            if (account == null)
                throw new ArgumentNullException(nameof(userID), $"{nameof(account)} cannot be found.");

            return account;
        }
        public async Task<bool> CreateAccount(Guid userID)
        {
            var existingAccount = (await _unitOfWork.Accounts.GetByConditionAsync(a => a.UserID == userID)).SingleOrDefault();
            if (existingAccount != null)
            {
                throw new InvalidOperationException("Account already exists for this user.");
            }

            var newAccount = new AccountEntity
            {
                UserID = userID,
                Balance = 0,
                ReservedBalance = 0,
            };

            await _unitOfWork.Accounts.AddAsync(newAccount);
            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }
        public async Task<bool> DeleteAccount(Guid userID)
        {
            var account = await GetAccountByUserID(userID);
            if (account == null)
                throw new ArgumentNullException(nameof(userID), "Account not found.");

            await _unitOfWork.Accounts.DeleteAsync(account);
            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }
        public async Task<bool> Deposit(Guid userID, decimal amount)
        {
            var account = await GetAccountByUserID(userID);

            account.Balance += amount;
            var result = await _unitOfWork.CommitAsync();

            return result >= 1;
        }

        public async Task<bool> Withdraw(Guid userID, decimal amout)
        {
            if (amout <= 0)
                throw new ArgumentException(nameof(amout), "Amount must be positive");

            var account = await GetAccountByUserID(userID);

            if (account.Balance - amout >= 0)
                account.Balance -= amout;

            else
            {
                throw new ArgumentException(nameof(amout), "Not enough balance at account.");
            }
            var result = await _unitOfWork.CommitAsync();

            return result >= 1;
        }

        public async Task<decimal> Balance(Guid userID)
        {
            var account = await GetAccountByUserID(userID);
            return account.Balance;
        }

        public async Task<decimal> ReservedBalance(Guid userID)
        {
            var account = await GetAccountByUserID(userID);
            return account.ReservedBalance;
        }
        public async Task<UserEntity> GetUserInfo(Guid userID)
        {
            var account = await GetAccountByUserID(userID);

            return account.User;
        }
        public async Task<IEnumerable<UserStockEntity>> GetStocks(Guid userID)
        {
            var userStocks = await _unitOfWork.UserStocks.GetByConditionAsync(s => s.UserID == userID);

            if (userStocks == null || !userStocks.Any())
                return Enumerable.Empty<UserStockEntity>();

            var aggregatedStocks = userStocks.ToList();

            return aggregatedStocks;
        }
    }
}
