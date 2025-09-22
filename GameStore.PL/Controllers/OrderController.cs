using GameStore.BLL.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.PL.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            if(orders == null)
            {
                ViewBag.ErrorMessage = "There is no orders";
                return View();
            }
            return View(orders);
        }
    }
}
