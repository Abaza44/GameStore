using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.Repo.Implementations
{
    public class OrderRepo : IOrderRepo
    {
        private readonly GameStoreContext _context;
        public OrderRepo(GameStoreContext context) => _context = context;

        public int GetOrdersCount() => _context.Orders.Count();

        public int GetOrdersCountForUser(int userId)
            => _context.Orders.Count(o => o.UserId == userId);

        public IEnumerable<Order> GetAllOrders(int take = 10)
            => _context.Orders
                       .Include(o => o.Items)
                           .ThenInclude(i => i.Game)
                       .OrderByDescending(o => o.OrderDate)
                       .Take(take)
                       .AsNoTracking()
                       .ToList();

        public IEnumerable<Order> GetUserOrders(int userId, OrderStatus? status = null)
        {
            var q = _context.Orders
                            .Include(o => o.Items)
                                .ThenInclude(i => i.Game)
                            .Where(o => o.UserId == userId);

            if (status.HasValue) q = q.Where(o => o.Status == status.Value);

            return q.OrderByDescending(o => o.OrderDate)
                    .AsNoTracking()
                    .ToList();
        }

        public Order? GetById(int orderId)
            => _context.Orders.AsNoTracking().FirstOrDefault(o => o.Id == orderId);

        public Order? GetByIdWithItems(int orderId)
            => _context.Orders
                       .Include(o => o.Items)
                           .ThenInclude(i => i.Game)
                       .AsNoTracking()
                       .FirstOrDefault(o => o.Id == orderId);

        public void Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public int CountByStatus(OrderStatus status)
            => _context.Orders.Count(o => o.Status == status);

        public Order? GetPendingOrderForUser(int userId)
            => _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Game)
            .FirstOrDefault(o => o.UserId == userId && o.Status == OrderStatus.Pending);
        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}