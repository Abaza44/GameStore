using GameStore.BLL.Services.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayPal.Api;


using OrderEntity = GameStore.DAL.Entities.Order;
using PaymentEntity = GameStore.DAL.Entities.Payment;

namespace GameStore.BLL.Services.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly GameStoreContext _context;
        private readonly IConfiguration _config;

        public CheckoutService(GameStoreContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private APIContext GetAPIContext()
        {
            var clientId = _config["PayPal:ClientId"];
            var secret = _config["PayPal:Secret"];
            var config = new Dictionary<string, string> { { "mode", "sandbox" } };

            var token = new OAuthTokenCredential(clientId, secret, config).GetAccessToken();
            return new APIContext(token) { Config = config };
        }

        public async Task<(int orderId, string approvalUrl)> CreateCheckout(int userId, int gameId, string baseUrl)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) throw new Exception("Game not found");

            
            var order = new OrderEntity
            {
                UserId = userId,
                TotalAmount = game.Price,
                Status = OrderStatus.Pending
            };
            _context.Orders.Add(order);

           
            var item = new OrderItem
            {
                Order = order,
                GameId = gameId,
                UnitPrice = game.Price
            };
            _context.OrderItems.Add(item);

            await _context.SaveChangesAsync();

            
            var apiContext = GetAPIContext();

            var payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = $"شراء لعبة {game.Title}",
                        amount = new Amount
                        {
                            currency = "USD",
                            total = game.Price.ToString("F2")
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

        public async Task<OrderEntity> ConfirmCheckout(int orderId, string paymentId, string payerId)
        {
            var apiContext = GetAPIContext();

            var paymentExecution = new PaymentExecution { payer_id = payerId };
            var executedPayment = new PayPal.Api.Payment { id = paymentId }
                .Execute(apiContext, paymentExecution);

            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

           
            order.Status = OrderStatus.Completed;

            var payEntity = new PaymentEntity
            {
                OrderId = order.Id,
                TransactionId = executedPayment.id,
                Provider = PaymentProvider.Paypal, 
                Status = PaymentStatus.Success      
            };

            _context.Payments.Add(payEntity);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}