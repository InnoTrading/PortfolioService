using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using System.Linq.Expressions;

namespace PortfolioService.Domain.Managers
{
    public class PortfolioManager(IUnitOfWork unitOfWork) : IPortfolioManager
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<UserStockEntity>> GetStocksByCondition(
            Expression<Func<UserStockEntity, bool>> predicate)
        {
            var stocks = await _unitOfWork.UserStocks.GetByConditionAsync(predicate);
            return stocks?.ToList() ?? Enumerable.Empty<UserStockEntity>();
        }

        public async Task<IEnumerable<UserStockEntity>> GetStocks(string Auth0UserId)
        {
            return await GetStocksByCondition(s => s.Auth0UserID == Auth0UserId);
        }

        public async Task<bool> AddStocks(string auth0UserId, string stockTicker, int quantity)
        {
            var userStocks = (await _unitOfWork.UserStocks
                .GetByConditionAsync(us =>
                    us.Auth0UserID == auth0UserId &&
                    us.StockTicker == stockTicker))
                .ToList();

            var existing = userStocks.SingleOrDefault();
            if (existing != null)
            {
                existing.Quantity += quantity;
                await _unitOfWork.UserStocks.UpdateAsync(existing);
            }
            else
            {
                var newStock = new UserStockEntity
                {
                    Auth0UserID = auth0UserId,
                    StockTicker = stockTicker,
                    Quantity = quantity
                };
                await _unitOfWork.UserStocks.AddAsync(newStock);
            }

            var saved = await _unitOfWork.CommitAsync();
            return saved >= 1;
        }

        public async Task<bool> RemoveStocks(string auth0UserId, string stockTicker, int quantityToSell)
        {
            var holdings = (await _unitOfWork.UserStocks
                .GetByConditionAsync(us =>
                    us.Auth0UserID == auth0UserId &&
                    us.StockTicker == stockTicker))
                .OrderBy(us => us.Id)
                .ToList();

            if (!holdings.Any())
                throw new InvalidOperationException($"User has no holdings of '{stockTicker}'.");

            var totalQuantity = holdings.Sum(h => h.Quantity);
            if (totalQuantity < quantityToSell)
                throw new InvalidOperationException(
                    $"Not enough shares to sell: have {totalQuantity}, need {quantityToSell}.");

            foreach (var h in holdings)
            {
                if (quantityToSell <= 0) break;

                if (h.Quantity <= quantityToSell)
                {
                    quantityToSell -= h.Quantity;
                    await _unitOfWork.UserStocks.DeleteAsync(h);
                }
                else
                {
                    h.Quantity -= quantityToSell;
                    quantityToSell = 0;
                    await _unitOfWork.UserStocks.UpdateAsync(h);
                }
            }

            var saved = await _unitOfWork.CommitAsync();
            return saved >= 1;
        }

    }
}