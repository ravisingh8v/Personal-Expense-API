namespace ExpenseTracker.Api.DTOs
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = "";
        public string? ColorCode { get; set; }
    }
}