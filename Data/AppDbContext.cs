using ExpenseTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Expense>().HasOne(e => e.Book).WithMany(b => b.Expenses).HasForeignKey(e => e.BookId).OnDelete(DeleteBehavior.Cascade);

            // Seed Categories (system defaults)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "food", ColorCode = "#22C55E", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 2, Name = "travel", ColorCode = "#3B82F6", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 3, Name = "shopping", ColorCode = "#A855F7", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 4, Name = "salary", ColorCode = "#10B981", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 5, Name = "bills", ColorCode = "#F97316", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Category { Id = 6, Name = "other", ColorCode = "#6B7280", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            // Seed Payment Types (e.g., payment methods)
            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { Id = 1, Name = "cash", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new PaymentType { Id = 2, Name = "bank transfer", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new PaymentType { Id = 3, Name = "card", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new PaymentType { Id = 4, Name = "upi", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            // Seed Transaction Types (income/expense)
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType { Id = 1, Name = "income", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate },
                new TransactionType { Id = 2, Name = "expense", IsDefault = true, UserId = null, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
        }

    }
}