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

        public async Task<bool> AddStocks(string Auth0UserId, Guid stockId, int quantity)
        {
            var existingStocks =
                await _unitOfWork.UserStocks.GetByConditionAsync(s => s.Auth0UserID == Auth0UserId && s.StockID == stockId);
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
                    Auth0UserID = Auth0UserId,
                    StockID = stockId,
                    Quantity = quantity
                };
                await _unitOfWork.UserStocks.AddAsync(newStock);
            }

            var result = await _unitOfWork.CommitAsync();
            return result >= 1;
        }

        public async Task<bool> RemoveStocks(string Auth0UserId, Guid stockId, int quantityToSell)
        {
            var userStocks = await GetStocksByCondition(s => s.Auth0UserID == Auth0UserId && s.StockID == stockId);

            var userStockEntities = userStocks as UserStockEntity[] ?? userStocks.ToArray();
            if (userStocks == null || !userStockEntities.Any())
                throw new ArgumentNullException(nameof(stockId), "Stock not found in portfolio.");

            var totalQuantity = userStockEntities.Sum(s => s.Quantity);
            if (totalQuantity < quantityToSell)
                throw new InvalidOperationException("Not enough stocks for sale in portfolio.");

            foreach (var stock in userStockEntities.OrderBy(s => s.Id))
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