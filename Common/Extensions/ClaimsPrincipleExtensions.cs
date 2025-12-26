using System.Security.Claims;

namespace ExpenseTracker.Api.Common.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException("User context is missing");
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is no authenticated");

            return userId;
        }
    }
}