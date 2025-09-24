using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
using GameStore.BLL.ModelVM;
using GameStore.BLL.ModelVM.cart;
using GameStore.BLL.Service.Abstractions;

namespace GameStore.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IGameRepo _gameRepo;

        public CartService(IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IGameRepo gameRepo)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _gameRepo = gameRepo;
        }

        public CartVM GetOrCreateCart(int userId)
        {
            var cart = _orderRepo.GetUserOrders(userId, OrderStatus.Pending).FirstOrDefault();

            if (cart == null)
            {
                cart = new Order
                {
                    UserId = userId,
                    Status = OrderStatus.Pending,
                    TotalAmount = 0
                };
                _orderRepo.Create(cart);
            }

            return new CartVM
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i =>
                {
                    var game = _gameRepo.GetById(i.GameId);  // fetch game safely
                    return new CartItemVM
                    {
                        GameId = i.GameId,
                        Title = game?.Title ?? "Unknown Game",
                        Price = i.UnitPrice
                    };
                }).ToList(),
                Total = cart.TotalAmount
            };
        }

        public void AddToCart(int userId, int gameId)
        {
            var cart = GetOrCreateCart(userId);
            var game = _gameRepo.GetById(gameId);
            var inaCart = cart.Items.Any(x => x.GameId == gameId);
            if (game == null || game.Status != GameStatus.Approved||inaCart) return;

            _orderItemRepo.AddItem(cart.CartId, gameId);
        }

        public void RemoveFromCart(int userId, int gameId)
        {
            var cart = GetOrCreateCart(userId);
            _orderItemRepo.DeleteItem(cart.CartId, gameId);
            var inaCart = cart.Items.Any(x => x.GameId == gameId);

            if (!cart.Items.Any()) _orderRepo.Delete(cart.CartId);
        }

        public CartVM GetCart(int userId)
        {
            var cart = _orderRepo
                .GetUserOrders(userId, OrderStatus.Pending)
                .FirstOrDefault();

            if (cart == null)
                return new CartVM { Items = new List<CartItemVM>(), Total = 0 };

            return new CartVM
            {
                Items = cart.Items.Select(i =>
                {
                    var game = _gameRepo.GetById(i.GameId);  // fetch game safely
                    return new CartItemVM
                    {
                        GameId = i.GameId,
                        Title = game?.Title ?? "Unknown Game",
                        Price = i.UnitPrice
                    };
                }).ToList(),
                Total = cart.TotalAmount
            };
        }

        public void ClearCart(int userId)
        {
            var cart = _orderRepo
                .GetUserOrders(userId, OrderStatus.Pending)
                .FirstOrDefault();
            if (cart == null) return;

            foreach (var item in cart.Items.ToList())
            {
                _orderItemRepo.DeleteItem(cart.Id, item.GameId);
            }
            _orderRepo.Delete(cart.Id);
        }
    }
}
