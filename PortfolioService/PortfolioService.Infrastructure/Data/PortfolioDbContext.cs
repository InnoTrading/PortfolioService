using Microsoft.EntityFrameworkCore;
using PortfolioService.Domain.Entities;

namespace PortfolioService.Infrastructure.Data;

public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
    public DbSet<UserStockEntity> UserStocks { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseNpgsql("Host=localhost;Database=portfolio_db;Username=portfolio_user;Password=StrongP123!");

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>().ToTable("Accounts");
        modelBuilder.Entity<UserStockEntity>().ToTable("UserStocks");

        modelBuilder.Entity<AccountEntity>(entity =>
        {
            entity.Property(e => e.Auth0UserId)
                .IsRequired();

            entity.Property(e => e.Balance)
                .IsRequired();

            entity.Property(e => e.ReservedBalance)
                .IsRequired();

        });

        modelBuilder.Entity<UserStockEntity>(entity =>
        {

            entity.Property(e => e.Auth0UserID);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.Property(e => e.StockTicker)
                .IsRequired();
                
        });

        base.OnModelCreating(modelBuilder);
    }
}
