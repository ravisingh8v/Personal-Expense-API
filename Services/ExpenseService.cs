using AutoMapper;
using ExpenseTracker.Api.Common.Exceptions;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Enums;
using ExpenseTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services
{
    public class ExpenseService(AppDbContext db, IMapper mapper) : IExpenseService
    {
        private readonly AppDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        public async Task<ExpenseCreateResponseDto> GetByIdAsync(string userId, int bookId, int expenseId)
        {
            var expense = await _db.Expenses.Include(e => e.Book).Include(e => e.Category).Include(e => e.TransactionType).Include(e => e.PaymentType).FirstOrDefaultAsync(e => e.Id == expenseId && e.BookId == bookId);
            if (expense == null) throw new BadRequestException("Invalid expense id");
            if (expense.Book?.UserId != userId) throw new ForbiddenException();

            return _mapper.Map<ExpenseCreateResponseDto>(expense);


        }
        public async Task<ExpenseCreateResponseDto> AddAsync(string userId, int bookId, CreateExpenseDto request)
        {
            // Validate Book ownership
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == userId);
            if (book == null) throw new ForbiddenException();

            // Validate Category
            if (!await _db.Categories.AnyAsync(c => c.Id == request.CategoryId && c.DeletedDate == null))
                throw new BadRequestException("Invalid category");

            // Validate PaymentType
            if (!await _db.PaymentTypes.AnyAsync(t => t.Id == request.PaymentTypeId && t.DeletedDate == null))
                throw new BadRequestException("Invalid expense type");

            // Validate TransactionType
            if (!await _db.TransactionTypes.AnyAsync(t => t.Id == request.TransactionTypeId && t.DeletedDate == null))
                throw new BadRequestException("Invalid transaction type");

            var expense = new Expense
            {
                BookId = bookId,
                Amount = request.Amount,
                Date = request.Date,
                Notes = request.Notes,
                CategoryId = request.CategoryId,
                PaymentTypeId = request.PaymentTypeId,
                TransactionTypeId = request.TransactionTypeId
            };

            _db.Expenses.Add(expense);

            await _db.SaveChangesAsync();

            // Fetching details of created expense 
            var createdExpense = await _db.Expenses
                .Include(e => e.Category)
                .Include(e => e.PaymentType)
                .Include(e => e.TransactionType)
                .FirstAsync(e => e.Id == expense.Id);

            return _mapper.Map<ExpenseCreateResponseDto>(createdExpense);
        }

        public async Task<ExpenseCreateResponseDto> UpdateAsync(string userId, int id, ExpenseUpdateDto req)
        {
            var expense = await _db.Expenses
                .Include(e => e.Book)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null) throw new NotFoundException("Expense not found");
            if (expense.Book?.UserId != userId) throw new ForbiddenException();

            if (expense.CategoryId != req.CategoryId && !await _db.Categories.AnyAsync(c => c.Id == req.CategoryId && c.DeletedDate == null))
                throw new BadRequestException("Invalid category");

            // Validate PaymentType
            if (expense.PaymentTypeId != req.PaymentTypeId && !await _db.PaymentTypes.AnyAsync(t => t.Id == req.PaymentTypeId && t.DeletedDate == null))
                throw new BadRequestException("Invalid payment type");

            // Validate TransactionType
            if (expense.TransactionTypeId != req.TransactionTypeId && !await _db.TransactionTypes.AnyAsync(t => t.Id == req.TransactionTypeId && t.DeletedDate == null))
                throw new BadRequestException("Invalid transaction type");

            // Apply updates
            expense.Amount = req.Amount;
            expense.Date = req.Date;
            expense.Notes = req.Notes;
            expense.CategoryId = req.CategoryId;
            expense.PaymentTypeId = req.PaymentTypeId;
            expense.TransactionTypeId = req.TransactionTypeId;
            expense.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return _mapper.Map<ExpenseCreateResponseDto>(expense);
        }

        public async Task<bool> DeleteAsync(string userId, int id)
        {
            var expense = await _db.Expenses
                .Include(e => e.Book)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null) throw new NotFoundException("Expense not found");
            if (expense?.Book?.UserId != userId) throw new ForbiddenException();

            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ExpenseListResponseDto> GetByBookAsync(string userId, int bookId)
        {
            var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == userId);
            if (book == null) throw new BadRequestException("Invalid book or unauthorized");

            var expenses = await _db.Expenses
                .Include(e => e.Category)
                .Include(e => e.PaymentType)
                .Include(e => e.TransactionType)
                .Where(e => e.BookId == book.Id)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return new ExpenseListResponseDto
            {
                Book = _mapper.Map<IdNameDto>(book),
                Expenses = _mapper.Map<List<ExpenseItemDto>>(expenses),
                Summary = new ExpenseSummaryDto
                {
                    TotalEntries = expenses.Count,
                    TotalExpense = expenses.Where(x => x.TransactionTypeId == (int)TransactionNature.Expense).Sum(x => x.Amount),
                    TotalIncome = expenses.Where(x => x.TransactionTypeId == (int)TransactionNature.Income).Sum(x => x.Amount),
                }
            };
        }
    }

}