using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;

namespace PortfolioService.Infrastructure.Repositories
{
    public class UnitOfWork(PortfolioDbContext context) : IUnitOfWork
    {
        private IRepository<AccountEntity>? _accounts;
        private IRepository<UserEntity>? _users;
        private IRepository<StockEntity>? _stocks;
        private IRepository<UserStockEntity>? _userStocks;

        public IRepository<AccountEntity> Accounts => _accounts ??= new Repository<AccountEntity>(context);
        public IRepository<UserEntity> Users => _users ??= new Repository<UserEntity>(context);
        public IRepository<StockEntity> Stocks => _stocks ??= new Repository<StockEntity>(context);
        public IRepository<UserStockEntity> UserStocks => _userStocks ??= new Repository<UserStockEntity>(context);

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}