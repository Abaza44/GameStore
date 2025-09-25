using System.Security.Claims;
using GameStore.BLL.ModelVM.Order;
using GameStore.BLL.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "User,Customer")]
public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService;
    private readonly IGameService _gameService;

    public CheckoutController(ICheckoutService checkoutService, IGameService gameService)
    {
        _checkoutService = checkoutService;
        _gameService = gameService;
    }

    // صفحة عرض Checkout (Cart Summary + Total)
    [HttpGet]
    public IActionResult Index(List<int> gameIds)
    {
        if (gameIds == null || !gameIds.Any())
            return RedirectToAction("Index", "Cart");

        // الأفضل تعمل GetByIds في الـ Service بدل ما تجيب الكل وتفلتر
        var games = _gameService.GetAll()
                                .Where(g => gameIds.Contains(g.Id))
                                .ToList();

        var orderVm = new OrderVM
        {
            TotalAmount = games.Sum(g => g.Price),
            Games = games.Select(g => g.Title).ToList(),
            Status = "Preparing"
        };

        return View(orderVm);
    }

    // تنفيذ الدفع
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Pay(OrderCreateModel model)
    {
        if (model.GameIds == null || !model.GameIds.Any())
            return RedirectToAction("Index", "Cart");

        // ناخد الـ UserId من الـ Claims (مش من الفورم – أأمن)
        var uidStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uidStr))
            return RedirectToAction("Login", "Account");

        model.UserId = int.Parse(uidStr);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var (orderId, approvalUrl) = await _checkoutService.CreateCheckout(model, baseUrl);

        return Redirect(approvalUrl); // المستخدم يتحوّل لصفحة PayPal
    }

    // نجاح الدفع
    [HttpGet]
    public async Task<IActionResult> Success(int orderId, string paymentId, string PayerID)
    {
        var orderVm = await _checkoutService.ConfirmCheckout(orderId, paymentId, PayerID);
        return View(orderVm); // تعرض تفاصيل الأوردر والدفع
    }

    // إلغاء الدفع
    [HttpGet]
    public IActionResult Cancel()
    {
        TempData["ErrorMessage"] = "Payment request cancelled.";
        return RedirectToAction("Index", "Cart");
    }
}