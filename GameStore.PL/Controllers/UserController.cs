//using GameStore.BLL.Service.Implementations;
using GameStore.BLL.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using GameStore.BLL.Service.Abstractions;

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
        public IActionResult GetUserLibrary(int id)
        {
            var games = _userLibraryService.GetUserGameswithCategory(id);
            return View("MyGames",games);
        }

    }
}
