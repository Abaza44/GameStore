using GameStore.BLL.Service.Implementations;
using GameStore.BLL.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetUser(int id)
        {
            
            var user = _userService.GetById(id);
            return View(user);
        }

        

    }
}
