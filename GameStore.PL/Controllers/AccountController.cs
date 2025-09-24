using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameStore.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IWebHostEnvironment _environment;

        public AccountController(IAuthService authService, IWebHostEnvironment environment)
        {
            _authService = authService;
            _environment = environment;
        }

       
        [HttpGet]
        public IActionResult Register() => View();

        //=======================Regester======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
    string fullName,
    string email,
    string password,
    DateTime dateOfBirth,
    bool emailConfirmed = false)
        {
            var user = await _authService.RegisterAsync(
                fullName, email, password, dateOfBirth,
                null,                 // profile picture disabled for now
                UserRole.User,        // 🟢 ثابت User
                emailConfirmed);

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        // 👇 عرض صفحة تسجيل الدخول
        [HttpGet]
        public IActionResult Login() => View();

        // 👇 معالجة تسجيل الدخول
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.LoginAsync(email, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View();
            }

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        // 👇 تسجيل الخروج
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}