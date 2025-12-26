namespace ExpenseTracker.Api.Models
{
    public class Book : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";

        public string? Description { get; set; }
        public string UserId { get; set; } = "";  // Clerk UserId
        public decimal TotalAmount { get; set; } = 0;
        public List<Expense> Expenses { get; set; } = new();
    }
}