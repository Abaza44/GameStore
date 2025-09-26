using GameStore.BLL.ModelVM.Game;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GameStore.PL.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GamesController> _logger;
        private readonly ICategoryService _categoryService;

        public GamesController(IGameService gameService, ILogger<GamesController> logger, ICategoryService categoryService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous] // أي حد يقدر يشوف الألعاب

        public async Task<IActionResult> Index()
        {
            var category = _categoryService.GetAll();
            var games = await _gameService.GetApprovedAsync();
            var topGame = await _gameService.GetTopAsync(4);
            var recentGame = await _gameService.GetRecentAsync(4);
            var GroupedGame = new List<(string, IEnumerable<GameViewModel>)>();
            GroupedGame.Add(("Top Games", topGame));
            GroupedGame.Add(("Recent Games", recentGame));
            foreach (var item in category)
            {
                var groupedGame =  _gameService.GetApproved().Where(a => a.CategoryName.Equals(item.Name));
                var group = (item.Name, groupedGame);
                GroupedGame.Add(group);
            }
            return View((category,GroupedGame));
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
        public IActionResult GetAllGame(int categoryId = 0, decimal priceFrom = 0, decimal priceTo = 10000)
        {
            try
            {
                var games = _gameService.GetAll();
                if (categoryId != 0)
                {
                    var categoryName = _categoryService.GetById(categoryId).Name;
                    games = games.Where(a => a.CategoryName.Equals(categoryName));

                }
                
                games = games.Where(a => (a.Price >= priceFrom) && (a.Price <= priceTo)).ToList();

                return View(games);
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "❌ Game not found.";
            }
            return View();
        }
    }
}
