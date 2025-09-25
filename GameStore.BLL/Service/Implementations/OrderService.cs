using GameStore.BLL.ModelVM.Order;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
using System.Collections.Generic;

namespace GameStore.BLL.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;

        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public int GetOrdersCount()
        {
            return _orderRepo.GetOrdersCount();
        }

        public int GetOrdersCountForUser(int userId)
        {
            return _orderRepo.GetOrdersCountForUser(userId);
        }

        public IEnumerable<GetAllOrdersModel> GetAllOrders(int take = 10)
        {
            var AllOrders = new List<GetAllOrdersModel>();
            var getallorders = _orderRepo.GetAllOrders(take);
            foreach (var order in getallorders)
            {
                AllOrders.Add(new GetAllOrdersModel
                {
                    OrderID = order.Id,
                    UserEmail = order.User.Email,
                    UserName = order.User.FullName,
                    Status = order.Status,
                    OrderItem = order.Items
                });
            }
            return AllOrders;

        }

        public IEnumerable<GetUserOrdersModel> GetUserOrders(int userId, OrderStatus? status = null)
        {
            var getUserOrders = new List<GetUserOrdersModel>();
            var userOrders = _orderRepo.GetUserOrders(userId, status);
            foreach (var order in userOrders)
            {
                getUserOrders.Add(new GetUserOrdersModel
                {
                    OrderID = order.Id,
                    UserEmail = order.User.Email,
                    UserName = order.User.FullName,
                    Status = order.Status,
                    OrderItem = order.Items
                });
            }
            return getUserOrders;
        }


        public GetOrderModel? GetByIdWithItems(int orderId)
        {
            var order = _orderRepo.GetById(orderId);
            var OrderModel = new GetOrderModel
            {
                OrderID = order.Id,
                UserEmail = order.User.Email,
                UserName = order.User.FullName,
                Status = order.Status,
                OrderItem = order.Items
            };
            return OrderModel;
        }

        public void Create(OrderCreateModel _order)
        {
            var order = new Order
            {
                UserId = _order.UserId,
                TotalAmount = _order.TotalAmount
            };
            _orderRepo.Create(order);
        }
        public void Delete(int id)
        {
            _orderRepo.Delete(id);
        }

        public int CountByStatus(OrderStatus status)
        {
            return _orderRepo.CountByStatus(status);
        }
    }
}