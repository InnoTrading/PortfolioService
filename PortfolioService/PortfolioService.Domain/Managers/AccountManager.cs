using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using System.Linq.Expressions;

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
        public async Task<IEnumerable<UserStockEntity>> GetStocksByCondition(Expression<Func<UserStockEntity, bool>> predicate)
        {
            var stocks = await _unitOfWork.UserStocks.GetByConditionAsync(predicate);
            return stocks?.ToList() ?? Enumerable.Empty<UserStockEntity>();
        }

        public async Task<IEnumerable<UserStockEntity>> GetStocks(Guid userID)
        {
            return await GetStocksByCondition(s => s.UserID == userID);
        }
        public async Task<bool> AddStocks(Guid userID, Guid stockID, int quantity)
        {
            var existingStocks = await _unitOfWork.UserStocks.GetByConditionAsync(s => s.UserID == userID && s.StockID == stockID);
            var existingStock = existingStocks.FirstOrDefault();

            if (existingStock != null)
            {
                existingStock.Quantity += quantity;
                await _unitOfWork.UserStocks.UpdateAsync(existingStock);
            }
            else
            {
                var newStock = new UserStockEntity
                {
                    UserID = userID,
                    StockID = stockID,
                    Quantity = quantity
                };
                await _unitOfWork.UserStocks.AddAsync(newStock);
            }

            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }

        public async Task<bool> RemoveStock(Guid userID, Guid stockID, int quantityToSell)
        {
            var userStocks = await GetStocksByCondition(s => s.UserID == userID && s.StockID == stockID);

            if (userStocks == null || !userStocks.Any())
                throw new ArgumentNullException(nameof(stockID), "Stock not found in portfolio.");

            var totalQuantity = userStocks.Sum(s => s.Quantity);
            if (totalQuantity < quantityToSell)
                throw new InvalidOperationException("Not enough stocks for sale in portfolio.");

            foreach (var stock in userStocks.OrderBy(s => s.ID))
            {
                if (quantityToSell <= 0)
                    break;

                if (stock.Quantity <= quantityToSell)
                {
                    quantityToSell -= stock.Quantity;
                    await _unitOfWork.UserStocks.DeleteAsync(stock);
                }
                else
                {
                    stock.Quantity -= quantityToSell;
                    quantityToSell = 0;
                    await _unitOfWork.UserStocks.UpdateAsync(stock);
                }
            }

            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }
    }
}
