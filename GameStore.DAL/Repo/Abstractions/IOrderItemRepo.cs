using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repo.Abstractions
{
    public interface IOrderItemRepo
    {
        void DeleteItem(int orderId, int gameId);
        void AddItem(int orderId, int gameId);

        //void Clear(int orderId);

        // Reads
        bool Exists(int orderId, int gameId);
        OrderItem? GetItem(int orderId, int gameId);
        IEnumerable<OrderItem> GetOrderItemsWithGame(int orderId);
        IEnumerable<OrderItem> GetOrderItems(int orderId);
        decimal GetSubtotal(int orderId);
        bool ExistsByGameId(int gameId);
    }
}
