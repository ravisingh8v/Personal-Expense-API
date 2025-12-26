using System.Text.Json.Serialization;

namespace ExpenseTracker.Api.Models
{
    public class Expense : BaseEntity
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Expense Type FK (e.g., Food, Travel, Subscription)
        public int PaymentTypeId { get; set; }
        public PaymentType? PaymentType { get; set; }

        // Transaction Type FK (Cash, UPI, Bank, etc.)
        public int TransactionTypeId { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}