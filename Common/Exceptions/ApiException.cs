namespace ExpenseTracker.Api.Common.Exceptions
{
    public abstract class ApiException(string message, int statusCode) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}