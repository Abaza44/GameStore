using GameStore.DAL.Entities;
using GameStore.DAL.Enums;


namespace GameStore.DAL.Repo.Abstractions
{
    public interface IOrderRepo
    {
        int GetOrdersCount();
        int GetOrdersCountForUser(int userId);
        IEnumerable<Order> GetAllOrders(int take=10);//For Admin Dashboard
        IEnumerable<Order> GetUserOrders(int userId, OrderStatus? status = null);//For User Dashboard

        Order? GetById(int orderId);
        Order? GetByIdWithItems(int orderId);
        void Create(Order order);
        void Delete(int id);
        

        int CountByStatus(OrderStatus status);//For Admin Dashboard
    }
}
