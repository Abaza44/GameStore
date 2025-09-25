using GameStore.BLL.ModelVM.Game;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GameStore.PL.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameStoreContext _context;
        private readonly IGameService _gameService;

        
        public GamesController(GameStoreContext context, IGameService gameService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        public IActionResult Index()
        {
            var games = _context.Games
                .Where(g => g.Status == GameStatus.Approved)
                .AsNoTracking()
                .ToList(); // عمرها ما تبقى null
            return View(games);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Publisher")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteGame(int gameId)
        {
            try
            {
                var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(uidStr))
                {
                    TempData["ErrorMessage"] = "Please login.";
                    return RedirectToAction("Login", "Account");
                }

                var currentUserId = int.Parse(uidStr);
                var isAdmin = User.IsInRole("Admin");

                _gameService.DeleteGame(gameId, currentUserId, isAdmin);
                TempData["SuccessMessage"] = "Game deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            if (User.IsInRole("Publisher"))
                return RedirectToAction("MyGames", "Publisher");

            return RedirectToAction("ReviewGames", "Admin");
        }
    }
}
