using GameStore.BLL.ModelVM.Game;
using GameStore.BLL.Service.Abstractions;
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
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;
        public PublisherController(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        // عرض الألعاب الخاصة بالـ Publisher الحالي
        public IActionResult MyGames()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var games = _gameService.GetByPublisher(userId).ToList();

            return View(games);
        }

        // صفحة رفع لعبة جديدة
        [HttpGet]
        public IActionResult UploadGame()
        {
            ViewBag.Categories = _categoryService.GetAll().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadGame(GameCreateModel game)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            _gameService.AddGame(game, userId);
            game.PublisherId = userId;
            
            TempData["SuccessMessage"] = "🎮 Game uploaded. Waiting admin approval!";
            return RedirectToAction("MyGames");
        }
    }
}