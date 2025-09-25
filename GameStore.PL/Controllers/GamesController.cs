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
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService gameService, ILogger<GamesController> logger)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous] // أي حد يقدر يشوف الألعاب

        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetApprovedAsync(); 
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
            catch (UnauthorizedAccessException)
            {
                TempData["ErrorMessage"] = "❌ You can only delete your own games.";
            }
            catch (InvalidOperationException)
            {
                TempData["ErrorMessage"] = "❌ You cannot delete this game because it has been purchased or added to a library.";
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "❌ Game not found.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting game {GameId}", gameId);
                TempData["ErrorMessage"] = "Something went wrong while deleting the game.";
            }

            if (User.IsInRole("Publisher"))
                return RedirectToAction("MyGames", "Publisher");
            else if (User.IsInRole("Admin"))
                return RedirectToAction("ReviewGames", "Admin");

            return RedirectToAction("Index");
        }
    }
}
