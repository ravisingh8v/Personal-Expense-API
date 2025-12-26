namespace ExpenseTracker.Api.DTOs
{
    public class ExpenseSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetBalance => TotalIncome - TotalExpense;
        public int TotalEntries { get; set; }
    }
}