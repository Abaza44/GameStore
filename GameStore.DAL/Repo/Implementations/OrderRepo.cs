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

        public OrderRepo(GameStoreContext context)
        {
            _context = context;
        }

        

        public void Create(Order order)
        {
            this._context.Orders.Add(order);    
            this._context.SaveChanges();
        }
        public void Delete(int id)
        {
            var order = this._context.Orders.FirstOrDefault(order => order.Id == id);
            if (order == null) return;
            this._context.Orders.Remove(order);
            this._context.SaveChanges();
        }


        public Order? GetById(int orderId)
        {
            return _context.Orders.AsNoTracking().Where(o => o.Id == orderId).FirstOrDefault();
        }
        public Order? GetByIdWithItems(int orderId)
        {
            return _context.Orders.AsNoTracking().Include(o=>o.Items)
                .Where(o => o.Id == orderId && o.Status == Enums.OrderStatus.Pending).FirstOrDefault();
        }

        public int GetOrdersCount()
        {
            return _context.Orders.AsNoTracking().Count();
        }
        public int GetOrdersCountForUser(int userId)
        {
            return _context.Orders.AsNoTracking().Count(o => o.UserId == userId);
        }
        public int CountByStatus(OrderStatus status)
        {
            return _context.Orders.AsNoTracking().Count(o => o.Status == status);
        }

        public IEnumerable<Order> GetUserOrders(int userId, OrderStatus? status = null)
        {
            if(status == null)
            {
                return _context.Orders.AsNoTracking().Include(u => u.User).Include(i => i.Items).Where(o => o.UserId == userId).ToList();
            }
            return _context.Orders.AsNoTracking().Include(u => u.User).Include(i => i.Items).Where(o => o.UserId == userId && o.Status == status).ToList();
        }
        public IEnumerable<Order> GetAllOrders(int take = 10)
        {
            return _context.Orders.AsNoTracking().Include(u => u.User).Include(i => i.Items).ThenInclude(g => g.Game).OrderByDescending(o => o.OrderDate).Take(take).ToList();
        }
    }
}
