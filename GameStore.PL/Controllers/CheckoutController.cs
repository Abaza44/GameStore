using GameStore.BLL.Service;
using GameStore.BLL.Service.Implementations;
using GameStore.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Customer")]
public class CheckoutController : Controller
{
    private readonly CheckoutService _checkoutService;

    public CheckoutController(CheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    public async Task<IActionResult> Pay(int gameId)
    {
       
        var userId = int.Parse(User.FindFirst("UserId").Value);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var (orderId, approvalUrl) = await _checkoutService.CreateCheckout(userId, gameId, baseUrl);
        return Redirect(approvalUrl);
    }

    public async Task<IActionResult> Success(int orderId, string paymentId, string PayerID)
    {
        var order = await _checkoutService.ConfirmCheckout(orderId, paymentId, PayerID);
        return View(order);  
    }

    public IActionResult Cancel()
    {
        return View();
    }
}