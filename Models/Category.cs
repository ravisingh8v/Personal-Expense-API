namespace ExpenseTracker.Api.Models
{
    public class Category : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ColorCode { get; set; } = "#6B7280";

        // True = created by system, False = created by user
        public bool IsDefault { get; set; } = true;

        // For multi-user support (optional)
        public string? UserId { get; set; }
    }
}