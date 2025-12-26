using ExpenseTracker.Api.DTOs;

namespace ExpenseTracker.Api.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync(string userId);
        Task<CategoryDto> CreateAsync(string userId, CreateCategoryDto body);
        Task<bool> DeleteAsync(string userId, int categoryId);
    }
}