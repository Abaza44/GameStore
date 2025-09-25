using System.Security.Claims;
//using GameStore.BLL.ModelVM.Cart;
using GameStore.BLL.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.PL.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(uidStr)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(uidStr);
            var vm = _cartService.GetCart(userId);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(int gameId)
        {
            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(uidStr)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(uidStr);
            _cartService.AddToCart(userId, gameId);
            TempData["SuccessMessage"] = "One game has been added.";
            return Redirect(Request.Headers["Referer"].ToString() ?? Url.Action("Index", "Cart")!);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Remove(int gameId)
        {
            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(uidStr)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(uidStr);
            _cartService.RemoveFromCart(userId, gameId);
            TempData["SuccessMessage"] = "تم حذف اللعبة من السلة.";
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(uidStr)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(uidStr);
            _cartService.ClearCart(userId);
            TempData["SuccessMessage"] = "تم تفريغ السلة.";
            return RedirectToAction("Index");
        }
    }
}