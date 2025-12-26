namespace ExpenseTracker.Api.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }
    }
}