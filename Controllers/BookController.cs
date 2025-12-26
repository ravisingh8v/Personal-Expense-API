using System.Security.Claims;
using AutoMapper;
using ExpenseTracker.Api.Common.Extensions;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/book")]
    [Authorize]
    public class BookController(IBookService service) : ControllerBase
    {
        private readonly IBookService _service = service;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BookDto>>>> GetAll()
        {
            var userId = User.GetUserId();
            var books = await _service.GetAllAsync(userId);

            return Ok(ApiResponse<List<BookDto>>.Success(books));
        }
        [HttpGet("{bookId}")]
        public async Task<ActionResult<ApiResponse<BookDto>>> Get([FromRoute] int bookId)
        {
            var userId = User.GetUserId();
            var book = await _service.GetAsync(bookId, userId);
            return Ok(ApiResponse<BookDto>.Success(book));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BookCreateResponseDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<BookCreateResponseDto>>> Create([FromBody] CreateBookDto request)
        {
            var userId = User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var book = await _service.CreateAsync(userId, request);
            return Created("", ApiResponse<BookCreateResponseDto>.Success(book));
        }
        [HttpPut("{bookId}")]
        public async Task<ActionResult<ApiResponse<string>>> Update([FromRoute] int bookId, BookUpdateDto body)
        {
            var userId = User?.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _service.UpdateAsync(bookId, userId, body);
            return Ok(ApiResponse<string>.Success("Book Updated Successfully"));
        }


        [HttpDelete("{bookId}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete([FromRoute] int bookId)
        {
            var userId = User?.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _service.DeleteAsync(bookId, userId);
            return Ok(ApiResponse<string>.Success("Deleted"));
        }

    }
}