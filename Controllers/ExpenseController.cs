using ExpenseTracker.Api.Common.Extensions;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/book/{bookId}/expense")]
    [Authorize]
    public class ExpenseController(IExpenseService service) : ControllerBase
    {
        private readonly IExpenseService _service = service;

        [HttpGet("{expenseId}")]
        public async Task<ActionResult<ApiResponse<ExpenseCreateResponseDto>>> GetById([FromRoute] int bookId, [FromRoute] int expenseId)
        {
            var userId = User.GetUserId();

            var expense = await _service.GetByIdAsync(userId, bookId, expenseId);
            return Ok(ApiResponse<ExpenseCreateResponseDto>.Success(expense));
        }
        [HttpGet()]
        public async Task<ActionResult<ApiResponse<ExpenseListResponseDto>>> GetAll([FromRoute] int bookId)
        {
            var userId = User.GetUserId();

            var expenses = await _service.GetByBookAsync(userId, bookId);
            return Ok(ApiResponse<ExpenseListResponseDto>.Success(expenses));
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExpenseCreateResponseDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<ExpenseCreateResponseDto>>> Create([FromRoute] int bookId, CreateExpenseDto body)
        {
            var userId = User.GetUserId();

            var createdExpense = await _service.AddAsync(userId, bookId, body);
            return Created("", ApiResponse<ExpenseCreateResponseDto>.Success(createdExpense));
        }


        [HttpPut("{expenseId}")]
        public async Task<ActionResult<ApiResponse<ExpenseCreateResponseDto>>> Update([FromRoute] int expenseId, ExpenseUpdateDto body)
        {
            var userId = User.GetUserId();
            var updateExpense = await _service.UpdateAsync(userId, expenseId, body);

            return Ok(ApiResponse<ExpenseCreateResponseDto>.Success(updateExpense));
        }


        [HttpDelete("{expenseId}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete([FromRoute] int expenseId)
        {
            var userId = User.GetUserId();
            await _service.DeleteAsync(userId, expenseId);
            return Ok(ApiResponse<string>.Success("Deleted successfully"));
        }

    }
}