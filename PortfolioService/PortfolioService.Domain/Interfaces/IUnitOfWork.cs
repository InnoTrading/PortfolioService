using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AccountEntity> Accounts { get; }
        IRepository<StockEntity> Stocks { get; }
        IRepository<UserStockEntity> UserStocks { get; }

        Task<int> CommitAsync();
    }
}