using GameStore.DAL.Entities;

namespace GameStore.BLL.Service.Abstractions
{
    public interface ICheckoutService
    {
        Task<(int orderId, string approvalUrl)> CreateCheckout(int userId, int gameId, string baseUrl);
        Task<Order> ConfirmCheckout(int orderId, string paymentId, string payerId);
    }
}