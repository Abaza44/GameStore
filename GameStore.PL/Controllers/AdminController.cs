using GameStore.BLL.Service.Abstractions;
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

        public AdminController(IUserService userService, GameStoreContext context)
        {
            _userService = userService;
            _context = context;
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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