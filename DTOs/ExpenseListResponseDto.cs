namespace ExpenseTracker.Api.DTOs
{
    public class ExpenseListResponseDto
    {
        public IdNameDto Book { get; set; } = new();
        public ExpenseSummaryDto Summary { get; set; } = new();
        public List<ExpenseItemDto> Expenses { get; set; } = new();
    }
}