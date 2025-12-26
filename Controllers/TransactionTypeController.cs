using ExpenseTracker.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/transaction-type")]
    [Authorize]
    public class TransactionTypeController(AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _db.TransactionTypes.OrderBy(c => c.Name).ToListAsync();
            return Ok(types);
        }
    }
}