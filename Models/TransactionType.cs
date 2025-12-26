namespace ExpenseTracker.Api.Models
{
    public class TransactionType : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsDefault { get; set; } = true;

        // Null for system types, filled for user-created types
        public string? UserId { get; set; }
    }

}