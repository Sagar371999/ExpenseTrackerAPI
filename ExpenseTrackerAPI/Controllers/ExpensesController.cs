using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ExpenseTrackerAPI.DTOs;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetExpenses()
        {
            var expenses = await _context.Expenses
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                })
                .ToListAsync();

            return Ok(expenses);
        }*/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetExpenses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                })
                .ToListAsync();

            return Ok(expenses);
        }
        /*[HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDTO>> GetExpense(int id)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id == id)
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                })
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }*/
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDTO>> GetExpense(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            var expense = await _context.Expenses
                .Where(e => e.Id == id && e.UserId == userId)
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                })
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }
        [HttpPost]
        public async Task<ActionResult<ExpenseDTO>> PostExpense(CreateExpenseDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var expense = new Expense
            {
                Title = dto.Title,
                Amount = dto.Amount,
                Category = dto.Category,
                Date = DateTime.Now,
                UserId = int.Parse(userIdClaim.Value)
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var expenseDto = new ExpenseDTO
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount = expense.Amount,
                Category = expense.Category,
                Date = expense.Date
            };

            return CreatedAtAction(
                nameof(GetExpense),
                new { id = expense.Id },
                expenseDto);
        }

        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, UpdateExpenseDTO dto)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            expense.Title = dto.Title;
            expense.Amount = dto.Amount;
            expense.Category = dto.Category;
            expense.Date = dto.Date;

            await _context.SaveChangesAsync();

            return NoContent();
        }*/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, UpdateExpenseDTO dto)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            expense.Title = dto.Title;
            expense.Amount = dto.Amount;
            expense.Category = dto.Category;
            expense.Date = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
