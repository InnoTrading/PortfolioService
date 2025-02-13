using Microsoft.EntityFrameworkCore;
using PortfolioService.Domain.Entities;

namespace PortfolioService.Infrastructure.Data
{
    public class PortfolioDbContext: DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) {}

        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<StockEntity> Stocks { get; set; } = null!;
        public DbSet<UserStockEntity> UserStocks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseNpgsql("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStockEntity>()
                .HasOne(us => us.Users)   
                .WithMany()                
                .HasForeignKey(us => us.UserID)
                .IsRequired();

            modelBuilder.Entity<UserStockEntity>()
                .HasOne(us => us.Stock)      
                .WithMany()
                .HasForeignKey(us => us.StockID)
                .IsRequired();

            modelBuilder.Entity<AccountEntity>()
                .HasOne(a => a.User)          
                .WithOne()
                .HasForeignKey<AccountEntity>(a => a.UserID)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
