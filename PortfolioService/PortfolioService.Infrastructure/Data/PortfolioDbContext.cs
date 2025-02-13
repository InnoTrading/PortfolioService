using Microsoft.EntityFrameworkCore;
using PortfolioService.Domain.Entities;

namespace PortfolioService.Infrastructure.Data
{
    public class PortfolioDbContext: DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) {}

        public DbSet<Accounts> Accounts { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Stocks> Stocks { get; set; } = null!;
        public DbSet<UserStocks> UserStocks { get; set; } = null!;

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
            modelBuilder.Entity<UserStocks>()
                .HasOne(us => us.Users)   
                .WithMany()                
                .HasForeignKey(us => us.UserID)
                .IsRequired();

            modelBuilder.Entity<UserStocks>()
                .HasOne(us => us.Stock)      
                .WithMany()
                .HasForeignKey(us => us.StockID)
                .IsRequired();

            modelBuilder.Entity<Accounts>()
                .HasOne(a => a.User)          
                .WithOne()
                .HasForeignKey<Accounts>(a => a.UserID)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
