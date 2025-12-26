namespace ExpenseTracker.Api.DTOs
{
    public class ExpenseUpdateDto
    {
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }

        public int CategoryId { get; set; }
        public int PaymentTypeId { get; set; }
        public int TransactionTypeId { get; set; }
    }
}