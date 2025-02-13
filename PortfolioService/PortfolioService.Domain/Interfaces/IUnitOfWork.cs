using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AccountEntity> Accounts { get; }
        IRepository<Users> Users { get; }
        IRepository<Stocks> Stocks { get; }
        IRepository<UserStocks> UserStocks { get; }

        Task<int> CommitAsync();
    }

}
