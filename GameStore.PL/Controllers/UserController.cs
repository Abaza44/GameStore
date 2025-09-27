//using GameStore.BLL.Service.Implementations;
using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameStore.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserLibraryService _userLibraryService;

        public UserController(IUserService userService, IUserLibraryService userLibraryService)
        {
            _userService = userService;
            _userLibraryService = userLibraryService;
        }

        [HttpGet]
        public IActionResult GetUser(int id)
        {
            
            var user = _userService.GetById(id);
            return View(user);
        }

        [HttpGet]
        public IActionResult GetUserLibrary()
        {
            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(uidStr)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(uidStr);
            var games = _userLibraryService.GetUserGameswithCategory(userId).ToList();
            return View("MyGames",games);
        }

    }
}
