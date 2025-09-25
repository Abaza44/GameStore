using GameStore.BLL.ModelVM.Order;
using GameStore.DAL.Entities;
using PayPal.Api;

namespace GameStore.BLL.Service.Abstractions
{
    public interface ICheckoutService
    {
        Task<(int orderId, string approvalUrl)> CreateCheckout(OrderCreateModel model, string baseUrl);
        Task<OrderVM> ConfirmCheckout(int orderId, string paymentId, string payerId);
    }
}