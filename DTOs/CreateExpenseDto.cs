using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.DTOs
{
    public class CreateExpenseDto
    {
        // public int BookId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTimeOffset Date { get; set; } = DateTime.UtcNow;

        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }
        [Required]
        public int TransactionTypeId { get; set; }
        public string? Notes { get; set; }
    }

}