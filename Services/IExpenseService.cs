using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Models;

namespace ExpenseTracker.Api.Services
{
    public interface IExpenseService
    {
        public Task<ExpenseCreateResponseDto> GetByIdAsync(string userId, int bookId, int expenseId);
        public Task<ExpenseCreateResponseDto> AddAsync(string userId, int bookId, CreateExpenseDto request);
        public Task<ExpenseCreateResponseDto> UpdateAsync(string userId, int id, ExpenseUpdateDto request);
        Task<bool> DeleteAsync(string userId, int id);
        Task<ExpenseListResponseDto> GetByBookAsync(string userId, int bookId);
    }

}