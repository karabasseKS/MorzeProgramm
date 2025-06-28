using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MorzeProgramm.Data;
using MorzeProgramm.Models;
using System.Threading.Tasks;

namespace MorzeProgramm.Controllers
{
    [Authorize]
    public class ErrorStatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ErrorStatController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> LogError([FromBody] ErrorLogModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(model.Symbol))
                return BadRequest("Символ не указан");

            var userId = _userManager.GetUserId(User);

            var existing = await _context.ErrorStats
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Symbol == model.Symbol);

            if (existing != null)
            {
                existing.Count += 1;
            }
            else
            {
                _context.ErrorStats.Add(new ErrorStat
                {
                    UserId = userId,
                    Symbol = model.Symbol,
                    Count = 1
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // Получение ошибок пользователя (можно использовать во View)
        [HttpGet]
        public async Task<IActionResult> GetUserErrors()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = _userManager.GetUserId(User);

            var errors = await _context.ErrorStats
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Count)
                .ToListAsync();

            return View("UserErrors", errors); // Создай View UserErrors.cshtml при необходимости
        }
    }

    public class ErrorLogModel
    {
        public string Symbol { get; set; }
    }
}
