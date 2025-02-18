using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;
using PortfolioService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PortfolioDbContext _context;

    private IRepository<AccountEntity>? _accounts;
    private IRepository<UserEntity>? _users;
    private IRepository<StockEntity>? _stocks;
    private IRepository<UserStockEntity>? _userStocks;

    public UnitOfWork(PortfolioDbContext context)
    {
        _context = context;
    }

    public IRepository<AccountEntity> Accounts => _accounts ??= new Repository<AccountEntity>(_context);
    public IRepository<UserEntity> Users => _users ??= new Repository<UserEntity>(_context);
    public IRepository<StockEntity> Stocks => _stocks ??= new Repository<StockEntity>(_context);
    public IRepository<UserStockEntity> UserStocks => _userStocks ??= new Repository<UserStockEntity>(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
