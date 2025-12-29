
namespace ExpenseTracker.Api.DTOs.Common
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public ApiError? Error { get; set; } = null;

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>
            {
                Data = data,
                Error = null
            };
        }
    }
}