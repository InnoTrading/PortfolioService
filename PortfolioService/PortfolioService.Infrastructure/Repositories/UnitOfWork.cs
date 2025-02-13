using PortfolioService.Domain.Entities;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;
using PortfolioService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PortfolioDbContext _context;

    private IRepository<Accounts>? _accounts;
    private IRepository<Users>? _users;
    private IRepository<Stocks>? _stocks;
    private IRepository<UserStocks>? _userStocks;

    public UnitOfWork(PortfolioDbContext context)
    {
        _context = context;
    }

    public IRepository<Accounts> Accounts => _accounts ??= new Repository<Accounts>(_context);
    public IRepository<Users> Users => _users ??= new Repository<Users>(_context);
    public IRepository<Stocks> Stocks => _stocks ??= new Repository<Stocks>(_context);
    public IRepository<UserStocks> UserStocks => _userStocks ??= new Repository<UserStocks>(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
