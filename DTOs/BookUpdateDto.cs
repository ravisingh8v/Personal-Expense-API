namespace ExpenseTracker.Api.DTOs
{
    public class BookUpdateDto
    {
        public string Title { get; set; }            // required
        public string? Description { get; set; }    // optional
    }
}