using GameStore.BLL.ModelVM.Order;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Service.Abstractions
{
    public interface IOrderService
    {
        int GetOrdersCount();
        int GetOrdersCountForUser(int userId);
        IEnumerable<GetAllOrdersModel> GetAllOrders(int take = 10);//For Admin Dashboard
        IEnumerable<GetUserOrdersModel> GetUserOrders(int userId, OrderStatus? status = null);//For User Dashboard

        GetOrderModel? GetByIdWithItems(int orderId);
        void Create(OrderCreateModel order);
        void Delete(int id);

        int CountByStatus(OrderStatus status);//For Admin Dashboard
    }
}