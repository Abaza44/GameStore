using GameStore.BLL.ModelVM.Order;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayPal.Api;


using OrderEntity = GameStore.DAL.Entities.Order;
using PaymentEntity = GameStore.DAL.Entities.Payment;

namespace GameStore.BLL.Service.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IGameRepo _gameRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly IConfiguration _config;
        
        public CheckoutService(IGameRepo gameRepo, IOrderRepo orderRepo, IPaymentRepo paymentRepo, IConfiguration config)
        {
            _gameRepo = gameRepo;
            _orderRepo = orderRepo;
            _paymentRepo = paymentRepo;
            _config = config;
        }

        private APIContext GetAPIContext()
        {
            var clientId = _config["PayPal:ClientId"];
            var secret = _config["PayPal:Secret"];
            var mode = _config["PayPal:Mode"] ?? "sandbox";

            var config = new Dictionary<string, string> { { "mode", mode } };
            var token = new OAuthTokenCredential(clientId, secret, config).GetAccessToken();
            return new APIContext(token) { Config = config };
        }

        public async Task<(int orderId, string approvalUrl)> CreateCheckout(OrderCreateModel model, string baseUrl)
        {
            var games = await _gameRepo.GetByIdsAsync(model.GameIds);

            var total = games.Sum(g => g.Price);

            var order = new OrderEntity
            {
                UserId = model.UserId,
                Status = OrderStatus.Pending,
                TotalAmount = total,
                Items = games.Select(g => new OrderItem
                {
                    GameId = g.Id,
                    UnitPrice = g.Price
                }).ToList()
            };

            await _orderRepo.CreateAsync(order);
            await _orderRepo.SaveChangesAsync();

            var apiContext = GetAPIContext();

            var payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
        {
            new Transaction
            {
                description = "شراء ألعاب من GameStore",
                amount = new Amount
                {
                    currency = "USD",
                    total = total.ToString("F2")
                }
            }
        },
                redirect_urls = new RedirectUrls
                {
                    return_url = $"{baseUrl}/Checkout/Success?orderId={order.Id}",
                    cancel_url = $"{baseUrl}/Checkout/Cancel"
                }
            };

            var created = payment.Create(apiContext);
            var approvalUrl = created.links.First(x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase)).href;

            return (order.Id, approvalUrl);
        }

        public async Task<OrderVM> ConfirmCheckout(int orderId, string paymentId, string payerId)
        {
            var apiContext = GetAPIContext();

            var paymentExecution = new PaymentExecution { payer_id = payerId };
            var executedPayment = new PayPal.Api.Payment { id = paymentId }
                .Execute(apiContext, paymentExecution);

            var order = await _orderRepo.GetByIdWithItemsAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            order.Status = OrderStatus.Completed;
            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();

            var payEntity = new PaymentEntity
            {
                OrderId = order.Id,
                TransactionId = executedPayment.id,
                Provider = PaymentProvider.Paypal,
                Status = PaymentStatus.Success
            };

            await _paymentRepo.AddAsync(payEntity);
            await _paymentRepo.SaveChangesAsync();

            return new OrderVM
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                PaymentTransactionId = executedPayment.id
            };
        }




    }
}