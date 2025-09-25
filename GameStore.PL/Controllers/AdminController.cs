using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Implementations;
using GameStore.DAL.DB;
using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly GameStoreContext _context;
        private readonly IGameService _gameService;


        public AdminController(IUserService userService, GameStoreContext context, IGameService gameService)
        {
            _userService = userService;
            _context = context;
            _gameService = gameService;
        }
        public IActionResult ManageGames()
        {
            var games = _gameService.GetAllGamesAdmin();
            return View(games);
        }

        // ✅ Users Dashboard
        public IActionResult Dashboard()
        {
            var users = _userService.GetAll(); // أو GetUsers() حسب الـ Interface عندك
            return View(users);
        }

        [HttpPost]
        public IActionResult ChangeRole(int userId, UserRole role)
        {
            _userService.UpdateRole(userId, role);
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            _userService.Delete(userId);
            return RedirectToAction("Dashboard");
        }

        // ✅ Games Review Section
        public IActionResult ReviewGames()
        {
            var pendingGames = _context.Games
                .Include(g => g.Publisher) // عشان تعرف مين اللي رفع اللعبة
                .Where(g => g.Status == GameStatus.Pending)
                .ToList();

            return View(pendingGames);
        }

        [HttpPost]
        
        public IActionResult Approve(int gameId)
        {
            var game = _context.Games.Find(gameId);
            if (game != null)
            {
                game.Status = GameStatus.Approved;
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewGames");
        }

        [HttpPost]
        
        public IActionResult Reject(int gameId, string reason)
        {
            var game = _context.Games.Find(gameId);
            if (game != null)
            {
                game.Status = GameStatus.Rejected;
                game.RejectionReason = reason;
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewGames");
        }
    }
}