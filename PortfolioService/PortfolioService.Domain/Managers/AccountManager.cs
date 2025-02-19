using System.Linq.Expressions;
using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Domain.Managers
{
    public class AccountManager(IUnitOfWork unitOfWork) : IAccountManager
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private async Task<AccountEntity> GetAccountByUserId(Guid userId)
        {
            var account = (await _unitOfWork.Accounts.GetByConditionAsync(a => a.UserID == userId)).SingleOrDefault();

            if (account == null)
                throw new ArgumentNullException(nameof(userId), $"{nameof(account)} cannot be found.");

            return account;
        }

        public async Task<bool> CreateAccount(Guid userId)
        {
            var existingAccount =
                (await _unitOfWork.Accounts.GetByConditionAsync(a => a.UserID == userId)).SingleOrDefault();
                
            if (existingAccount != null)
            {
                throw new InvalidOperationException("Account already exists for this user.");
            }

            var newAccount = new AccountEntity
            {
                UserID = userId,
                Balance = 0,
                ReservedBalance = 0,
            };

            await _unitOfWork.Accounts.AddAsync(newAccount);
            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }

        public async Task<bool> DeleteAccount(Guid userId)
        {
            var account = await GetAccountByUserId(userId);
            if (account == null)
                throw new ArgumentNullException(nameof(userId), "Account not found.");

            await _unitOfWork.Accounts.DeleteAsync(account);
            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }

        public async Task<bool> Deposit(Guid userId, decimal amount)
        {
            var account = await GetAccountByUserId(userId);

            account.Balance += amount;
            var result = await _unitOfWork.CommitAsync();

            return result >= 1;
        }

        public async Task<bool> Withdrawal(Guid userId, decimal amout)
        {
            if (amout <= 0)
                throw new ArgumentException(nameof(amout), "Amount must be positive");

            var account = await GetAccountByUserId(userId);

            if (account.Balance - amout >= 0)
                account.Balance -= amout;

            else
            {
                throw new ArgumentException(nameof(amout), "Not enough balance at account.");
            }

            var result = await _unitOfWork.CommitAsync();

            return result >= 1;
        }

        public async Task<decimal> Balance(Guid userId)
        {
            var account = await GetAccountByUserId(userId);
            return account.Balance;
        }

        public async Task<decimal> ReservedBalance(Guid userId)
        {
            var account = await GetAccountByUserId(userId);
            return account.ReservedBalance;
        }

        public async Task<UserEntity> GetUserInfo(Guid userId)
        {
            var account = await GetAccountByUserId(userId);

            return account.User;
        }
    }
}