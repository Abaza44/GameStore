using GameStore.DAL.Entities;
using GameStore.DAL.Enums;


namespace GameStore.DAL.Repo.Abstractions
{
    public interface IOrderRepo
    {
        int GetOrdersCount();
        int GetOrdersCountForUser(int userId);
        IEnumerable<Order> GetAllOrders(int take = 10); 
        IEnumerable<Order> GetUserOrders(int userId, OrderStatus? status = null); 

        Order? GetById(int orderId);
        Order? GetByIdWithItems(int orderId);
        void Create(Order order);
        void Delete(int id);

        int CountByStatus(OrderStatus status); 
        Order? GetPendingOrderForUser(int userId);
        void Update(Order order);


        // ---------- New Async Versions ----------
        Task<int> GetOrdersCountAsync();
        Task<int> GetOrdersCountForUserAsync(int userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync(int take = 10);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId, OrderStatus? status = null);

        Task<Order?> GetByIdAsync(int orderId);
        Task<Order?> GetByIdWithItemsAsync(int orderId);
        Task CreateAsync(Order order);
        Task DeleteAsync(int id);

        Task<int> CountByStatusAsync(OrderStatus status);
        Task<Order?> GetPendingOrderForUserAsync(int userId);
        Task UpdateAsync(Order order);
        Task SaveChangesAsync();   
    }
}
