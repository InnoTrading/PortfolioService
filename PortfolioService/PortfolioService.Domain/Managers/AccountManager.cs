using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Domain.Managers
{
    public class AccountManager(IUnitOfWork unitOfWork) : IAccountManager
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private async Task<AccountEntity> GetAccountByUserId(string Auth0Id)
        {
            var account = (await _unitOfWork.Accounts.GetByConditionAsync(a => a.Auth0UserId == Auth0Id)).SingleOrDefault();

            if (account == null)
                throw new ArgumentNullException(nameof(Auth0Id), $"{nameof(account)} cannot be found.");

            return account;
        }

        public async Task<bool> CreateAccount(string auth0UserId)
        {
            var existingAccount = (await _unitOfWork.Accounts.GetByConditionAsync(a => a.Auth0UserId == auth0UserId)).SingleOrDefault();

            if (existingAccount != null)
            {
                throw new InvalidOperationException("Account already exists for this user.");
            }

            var newAccount = new AccountEntity
            {
                Auth0UserId = auth0UserId,
                Balance = 0,
                ReservedBalance = 0,
            };

            await _unitOfWork.Accounts.AddAsync(newAccount);
            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }

        public async Task<bool> DeleteAccount(string userId)
        {
            var account = await GetAccountByUserId(userId);
            if (account == null)
                throw new ArgumentNullException(nameof(userId), "Account not found.");

            await _unitOfWork.Accounts.DeleteAsync(account);
            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }

        public async Task<bool> Deposit(string userId, decimal amount)
        {
            var account = await GetAccountByUserId(userId);
            if (account == null)
            {
                await CreateAccount(userId);
            }

            account!.Balance += amount;
            var result = await _unitOfWork.CommitAsync();

            return result >= 1;
        }

        public async Task<bool> Withdrawal(string userId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException(nameof(amount), "Amount must be positive");

            var account = await GetAccountByUserId(userId);

            if (account.Balance - amount >= 0)
                account.Balance -= amount;
            else
            {
                throw new ArgumentException(nameof(amount), "Not enough balance at account.");
            }

            var result = await _unitOfWork.CommitAsync();

            return result >= 1;
        }

        public async Task<decimal> Balance(string userId)
        {
            var account = await GetAccountByUserId(userId);
            return account.Balance;
        }

        public async Task<decimal> ReservedBalance(string userId)
        {
            var account = await GetAccountByUserId(userId);
            return account.ReservedBalance;
        }
        public async Task<bool> ReserveBalance(string userId, decimal amount)
        {
            var account = await GetAccountByUserId(userId);
            account.ReservedBalance += amount;

            var result = await _unitOfWork.CommitAsync();

            return result > 0;
        }
        public async Task<bool> ReleaseReservedBalance(string userId, decimal amount)
        {
            var account = await GetAccountByUserId(userId);
            account.ReservedBalance -= amount;

            var result = await _unitOfWork.CommitAsync();

            return result > 0;
        }

    }
}
