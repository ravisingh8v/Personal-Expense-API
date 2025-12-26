namespace ExpenseTracker.Api.DTOs
{
    public class ExpenseItemDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Notes { get; set; }

        public ExpenseItemCategoryDto Category { get; set; } = new();
        public IdNameDto PaymentType { get; set; } = new();
        public IdNameDto TransactionType { get; set; } = new();
    }
}