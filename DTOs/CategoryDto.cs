namespace ExpenseTracker.Api.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; } = "#6B7280";
        public bool IsDefault { get; set; }
    }
}