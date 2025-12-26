namespace ExpenseTracker.Api.Models
{
    public class PaymentType : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; // cash, card, upi, etc.
        public bool IsDefault { get; set; } = true;
        // Null for system types, filled for user-created types
        public string? UserId { get; set; }
    }

}