namespace ExpenseTracker.Api.Common.Exceptions
{
    public class ForbiddenException() : ApiException("Forbidden", StatusCodes.Status403Forbidden)
    {

    }
}