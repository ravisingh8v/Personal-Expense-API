namespace ExpenseTracker.Api.Common.Exceptions
{
    public class UnauthorizedException() : ApiException("Unauthorized", StatusCodes.Status401Unauthorized)
    {

    }
}