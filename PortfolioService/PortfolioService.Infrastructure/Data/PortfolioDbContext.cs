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
            // Ustawienie właściwości ID jako klucza głównego dla encji dziedziczących z BaseEntity.
            modelBuilder.Entity<BaseEntity>()
                .HasKey(e => e.ID);

            // Konfiguracja encji UserEntity: wymagane pola oraz ograniczenie długości.
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.Property(e => e.UserName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.UserEmail)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            // Konfiguracja encji StockEntity: wymagane pola oraz ograniczenie długości dla Name i Ticker.
            modelBuilder.Entity<StockEntity>(entity =>
            {
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Ticker)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(e => e.Price)
                      .IsRequired();

                entity.Property(e => e.Description)
                      .IsRequired();
            });

            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.Property(e => e.Balance)
                      .IsRequired();

                entity.Property(e => e.ReservedBalance)
                      .IsRequired();

                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<AccountEntity>(e => e.UserID)
                      .IsRequired();
            });

            modelBuilder.Entity<UserStockEntity>(entity =>
            {
                entity.Property(e => e.Quantity)
                      .IsRequired();

                entity.HasOne(e => e.Users)
                      .WithMany()
                      .HasForeignKey(e => e.UserID)
                      .IsRequired();

                entity.HasOne(e => e.Stock)
                      .WithMany()
                      .HasForeignKey(e => e.StockID)
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
