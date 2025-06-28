using Microsoft.AspNetCore.Mvc;
using MorzeProgramm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MorzeProgramm.Data;

namespace MorseWebApp.Controllers
{
    public class ObuchenieController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        private static readonly Dictionary<int, List<string>> BlockLetters = new()
        {
            { 1, new() { "А", "Е", "Л", "Ж" } },
            { 2, new() { "Ц", "С", "Т", "Щ" } },
            { 3, new() { "Д", "О", "Р", "И" } },
            { 4, new() { "Г", "Ь", "Ф", "Н" } },
            { 5, new() { "Й", "У", "Х", "К" } },
            { 6, new() { "Б", "П", "М", "Ы" } },
            { 7, new() { "З", "В", "Ш", "Я" } },
            { 8, new() { "Ч", "Э", "Ю" } },
            { 9, new() { "1", "2", "3", "4", "5" } },
            { 10, new() { "6", "7", "8", "9", "0" } }
            // ❗ BlockId 11 — генерируется динамически ниже
        };

        public ObuchenieController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = BlockLetters.Select(pair => new BlockListItem
            {
                BlockId = pair.Key,
                Letters = pair.Value
            }).ToList();

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);

                // Получаем прогресс
                var progresses = await _context.UserProgresses
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                foreach (var block in model)
                {
                    var progress = progresses.FirstOrDefault(p => p.BlockId == block.BlockId);
                    if (progress != null)
                    {
                        block.ProgressPercent = progress.ProgressPercent;
                        block.IsCompleted = progress.IsCompleted;
                    }
                }

                // 🔥 Добавляем динамический блок 11: Частые ошибки
                var errorLetters = await _context.ErrorStats
                    .Where(e => e.UserId == userId && e.Count >= 2)
                    .OrderByDescending(e => e.Count)
                    .Select(e => e.Symbol)
                    .Distinct()
                    .ToListAsync();

                if (errorLetters.Any())
                {
                    model.Add(new BlockListItem
                    {
                        BlockId = 11,
                        Letters = errorLetters,
                        ProgressPercent = 0,
                        IsCompleted = false
                    });
                }
            }

            return View(model);
        }

        public IActionResult Block(int id)
        {
            // Блок 11 — это ошибки, обрабатываем отдельно
            if (id == 11)
            {
                var userId = _userManager.GetUserId(User);
                var errorLetters = _context.ErrorStats
                    .Where(e => e.UserId == userId && e.Count >= 2)
                    .OrderByDescending(e => e.Count)
                    .Select(e => e.Symbol)
                    .ToList();

                if (!errorLetters.Any())
                    return NotFound("Нет ошибок для тренировки");

                var errorModel = new BlockViewModel
                {
                    BlockId = 11,
                    Letters = errorLetters
                };

                return View("Block", errorModel);
            }

            // Обычные блоки
            if (!BlockLetters.TryGetValue(id, out var letters))
                return NotFound();

            var model = new BlockViewModel
            {
                BlockId = id,
                Letters = letters
            };

            return View("Block", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProgress([FromBody] ProgressSaveModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userId = _userManager.GetUserId(User);

            var existing = await _context.UserProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.BlockId == model.BlockId);

            if (existing != null)
            {
                existing.ProgressPercent = model.Percent;
                existing.IsCompleted = model.Percent >= 80;
            }
            else
            {
                _context.UserProgresses.Add(new UserProgress
                {
                    UserId = userId,
                    BlockId = model.BlockId,
                    ProgressPercent = model.Percent,
                    IsCompleted = model.Percent >= 80
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class ProgressSaveModel
    {
        public int BlockId { get; set; }
        public float Percent { get; set; }
    }
}
