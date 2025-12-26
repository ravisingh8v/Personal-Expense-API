namespace ExpenseTracker.Api.Common.Exceptions
{
    public class BadRequestException(string message) : ApiException(message, StatusCodes.Status400BadRequest)
    {

    }
}