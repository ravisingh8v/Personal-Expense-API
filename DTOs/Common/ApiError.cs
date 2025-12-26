namespace ExpenseTracker.Api.DTOs.Common
{
    public class ApiError
    {
        public string Message { get; set; } = "";
        public int StatusCode { get; set; }
    }
}