namespace ExpenseTracker.Api.DTOs
{
    public class BookCreateResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
    }
}