using AutoMapper;
using ExpenseTracker.Api.Common.Exceptions;
using ExpenseTracker.Api.Common.Validations;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services
{
    public class CategoryService(AppDbContext db, IMapper mapper) : ICategoryService
    {
        private readonly AppDbContext _db = db;
        private readonly IMapper _mapper = mapper;

        public async Task<List<CategoryDto>> GetAllAsync(string userId)
        {
            var categories = await _db.Categories.Where(c => (c.IsDefault || c.UserId == userId) && c.DeletedDate == null).OrderBy(c => c.Id).ToListAsync();

            return _mapper.Map<List<CategoryDto>>(categories);
        }


        public async Task<CategoryDto> CreateAsync(string userId, CreateCategoryDto body)
        {
            if (string.IsNullOrWhiteSpace(body.Name))
                throw new BadRequestException("Category is required");

            // Optional: prevent duplicate category names per user
            bool exists = await _db.Categories.AnyAsync(c =>
                c.Name == body.Name &&
                (c.IsDefault || c.UserId == userId));

            if (exists)
                throw new BadRequestException("Category already exists");

            if (body.ColorCode != null && !ColorValidator.IsValidHex(body.ColorCode))
                throw new BadRequestException("Invalid color code");

            var category = new Category
            {
                Name = body.Name,
                ColorCode = body.ColorCode ?? "#6B7280",
                IsDefault = false,
                UserId = userId
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }


        public async Task<bool> DeleteAsync(string userId, int categoryId)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null) throw new BadRequestException("Invalid Category");
            if (category.IsDefault) throw new BadRequestException("You cannot delete default categories");
            if (category.UserId != userId) throw new ForbiddenException();
            category.DeletedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return true;
        }

    }
}