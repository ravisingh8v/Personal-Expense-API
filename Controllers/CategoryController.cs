using System.Security.Claims;
using AutoMapper;
using ExpenseTracker.Api.Common.Extensions;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.DTOs;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/category")]
    [Authorize]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        private readonly ICategoryService _service = service;

        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAll()
        {
            var userId = User.GetUserId();
            var categories = await _service.GetAllAsync(userId);
            return Ok(ApiResponse<List<CategoryDto>>.Success(categories));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CreateCategoryDto category)
        {
            var userId = User.GetUserId();
            var cat = await _service.CreateAsync(userId, category);
            return Created(string.Empty, ApiResponse<CategoryDto>.Success(cat));
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<string>>> Delete([FromRoute] int id)
        {
            var userId = User.GetUserId();

            await _service.DeleteAsync(userId, id);
            return Ok(ApiResponse<string>.Success("Deleted"));
        }
    }
}