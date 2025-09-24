using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.PL.Controllers
{
    [Authorize(Roles = "Publisher")]
    public class PublisherController : Controller
    {
        private readonly GameStoreContext _context;

        public PublisherController(GameStoreContext context)
        {
            _context = context;
        }

        // عرض الألعاب الخاصة بالـ Publisher الحالي
        public IActionResult MyGames()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var games = _context.Games
                .Where(g => g.PublisherId == userId)
                .ToList();

            return View(games);
        }

        // صفحة رفع لعبة جديدة
        [HttpGet]
        
        public IActionResult UploadGame()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult UploadGame(string title, string description, string posterUrl, string downloadUrl, decimal price, int categoryId)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var game = new Game
            {
                Title = title,
                Description = description,
                PosterUrl = posterUrl,
                DownloadUrl = downloadUrl,
                Price = price,
                PublisherId = userId,
                CategoryId = categoryId,             // ✅ لازم Category صالح
                Status = GameStatus.Pending
            };

            _context.Games.Add(game);
            _context.SaveChanges();
            TempData["msg"] = "Game uploaded and waiting admin approval.";
            return RedirectToAction("MyGames");
        }
    }
}