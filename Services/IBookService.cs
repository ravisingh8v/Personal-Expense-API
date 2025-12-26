using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Models;

namespace ExpenseTracker.Api.Services
{
    public interface IBookService
    {
        public Task<List<BookDto>> GetAllAsync(string userId);
        public Task<BookDto> GetAsync(int id, string userId);
        public Task<BookCreateResponseDto> CreateAsync(string userId, CreateBookDto request);
        public Task<bool> UpdateAsync(int id, string userId, BookUpdateDto dto);
        public Task<bool> DeleteAsync(int id, string userId);
    }

}