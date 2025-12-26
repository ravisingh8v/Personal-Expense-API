using AutoMapper;
using ExpenseTracker.Api.Common.Exceptions;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Enums;
using ExpenseTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services
{
    public class BookService(AppDbContext db, IMapper mapper) : IBookService
    {
        private readonly AppDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<List<BookDto>> GetAllAsync(string userId)
        {
            var books = await _db.Books
                .Include(b => b.Expenses)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            // Auto-calc totals
            foreach (var book in books)
            {
                var expenses = book.Expenses;
                book.TotalAmount = expenses.Where(e => e.TransactionTypeId == (int)TransactionNature.Income).Sum(e => e.Amount) - expenses.Where(e => e.TransactionTypeId == (int)TransactionNature.Expense).Sum(e => e.Amount);
            }


            await _db.SaveChangesAsync();

            return _mapper.Map<List<BookDto>>(books);
        }
        public async Task<BookDto> GetAsync(int id, string userId)
        {
            var book = await _db.Books
                .Where(b => b.UserId == userId && b.Id == id)
                .FirstOrDefaultAsync();

            return book == null ? throw new NotFoundException("Book not found") : _mapper.Map<BookDto>(book);
        }


        public async Task<BookCreateResponseDto> CreateAsync(string userId, CreateBookDto request)
        {
            var book = new Book
            {
                Title = request.Title,
                Description = request.Description,
                UserId = userId
            };

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return _mapper.Map<BookCreateResponseDto>(book);
        }


        public async Task<bool> UpdateAsync(int id, string userId, BookUpdateDto dto)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) throw new NotFoundException("Book not found");
            if (book.UserId != userId) throw new ForbiddenException();

            // Update allowed fields
            book.Title = dto.Title.Trim();
            book.Description = string.IsNullOrWhiteSpace(dto.Description) ? book.Description : dto.Description.Trim();
            book.UpdatedAt = DateTime.UtcNow;

            _db.Books.Update(book); // not required but clear intention
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var book = await _db.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) throw new NotFoundException("Book not found");
            if (book.UserId != userId) throw new ForbiddenException();

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}