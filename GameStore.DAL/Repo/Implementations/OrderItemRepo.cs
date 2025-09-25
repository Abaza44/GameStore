using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace GameStore.DAL.Repo.Implementations
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly GameStoreContext _context;

        public OrderItemRepo(GameStoreContext context)
        {
            _context = context;
        }

        public void AddItem(int orderId, int gameId)
        {
            var order = _context.Orders.
                Where(o => o.Id == orderId && (o.Status == Enums.OrderStatus.Pending || o.Status == Enums.OrderStatus.Failed))
                .FirstOrDefault();
            var game = _context.Games.Where(g => g.Id == gameId).FirstOrDefault();

            

            if (order == null || game == null) return;
            if (order.Status != OrderStatus.Pending) return;
            bool exists = Exists(orderId,gameId);
            if (exists) return;
            order.TotalAmount += game.Price;
            
            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                GameId = gameId,
                UnitPrice = game.Price,
            };

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
        }
        public void DeleteItem(int orderId, int gameId)
        {
            var order = _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
            var game = _context.Games.Where(g => g.Id == gameId).FirstOrDefault();



            if (order == null || game == null) return;
            if (order.Status != OrderStatus.Pending) return;
            bool exists = Exists(orderId, gameId);
            if (!exists) return;
            order.TotalAmount -= game.Price;

            var orderItem = _context.OrderItems
                .Where(oi => oi.OrderId == orderId && oi.GameId == gameId).FirstOrDefault();

            if (orderItem == null) return;
            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
        }
        //public void Clear(int orderId)
        //{
        //    var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

        //    if (order == null || order.Status != OrderStatus.Pending)
        //        return;

        //    var items = _context.OrderItems.Where(i => i.OrderId == orderId).ToList();
        //    if (items.Count == 0) return;

        //    _context.OrderItems.RemoveRange(items);
        //    _context.SaveChanges();
        //}

        

        public bool Exists(int orderId, int gameId)
        {
            return _context.OrderItems.AsNoTracking().Any(oi => oi.OrderId == orderId && oi.GameId == gameId);
        }
        public OrderItem? GetItem(int orderId, int gameId)
        {
            return _context.OrderItems.AsNoTracking().FirstOrDefault(oi => oi.OrderId == orderId && oi.GameId == gameId);
        }


        public IEnumerable<OrderItem> GetOrderItems(int orderId)
        {
            return _context.OrderItems.AsNoTracking().Where(oi => oi.OrderId == orderId).ToList();
        }
        public IEnumerable<OrderItem> GetOrderItemsWithGame(int orderId)
        {
            return _context.OrderItems.AsNoTracking().Where(oi => oi.OrderId == orderId).Include(o => o.Game).ToList();
        }


        public decimal GetSubtotal(int orderId)
        {
            return _context.OrderItems.AsNoTracking()
                .Where(oi => oi.OrderId == orderId).Sum(oi => oi.UnitPrice);
        }
        public bool ExistsByGameId(int gameId) => _context.OrderItems.Any(x => x.GameId == gameId);
        
    }
}
