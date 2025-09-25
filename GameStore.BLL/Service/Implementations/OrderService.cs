using GameStore.BLL.ModelVM.Order;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.BLL.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IGameRepo _gameRepo;
        private readonly IOrderItemRepo _orderItemRepo;

        public OrderService(IOrderRepo orderRepo, IGameRepo gameRepo, IOrderItemRepo orderItemRepo)
        {
            _orderRepo = orderRepo;
            _gameRepo = gameRepo;
            _orderItemRepo = orderItemRepo;
        }

        // ✅ إحصائيات عامة
        public int GetOrdersCount()
        {
            return _orderRepo.GetOrdersCount();
        }

        public int GetOrdersCountForUser(int userId)
        {
            return _orderRepo.GetOrdersCountForUser(userId);
        }

        // ✅ كله: للأدمن
        public IEnumerable<GetAllOrdersModel> GetAllOrders(int take = 10)
        {
            var AllOrders = new List<GetAllOrdersModel>();
            var getallorders = _orderRepo.GetAllOrders(take);

            foreach (var order in getallorders)
            {
                AllOrders.Add(new GetAllOrdersModel
                {
                    OrderID = order.Id,
                    UserEmail = order.User?.Email,
                    UserName = order.User?.FullName,
                    Status = order.Status,
                    OrderItem = order.Items
                });
            }

            return AllOrders;
        }

        // ✅ أوردرات مستخدم محدد
        public IEnumerable<GetUserOrdersModel> GetUserOrders(int userId, OrderStatus? status = null)
        {
            var getUserOrders = new List<GetUserOrdersModel>();
            var userOrders = _orderRepo.GetUserOrders(userId, status);

            foreach (var order in userOrders)
            {
                getUserOrders.Add(new GetUserOrdersModel
                {
                    OrderID = order.Id,
                    UserEmail = order.User?.Email,
                    UserName = order.User?.FullName,
                    Status = order.Status,
                    OrderItem = order.Items
                });
            }

            return getUserOrders;
        }

        // ✅ أوردر واحد بالتفاصيل
        public GetOrderModel? GetByIdWithItems(int orderId)
        {
            var order = _orderRepo.GetById(orderId);
            if (order == null) return null;

            var OrderModel = new GetOrderModel
            {
                OrderID = order.Id,
                UserEmail = order.User?.Email,
                UserName = order.User?.FullName,
                Status = order.Status,
                OrderItem = order.Items
            };
            return OrderModel;
        }

        // ✅ إنشاء أوردر جديد من OrderCreateModel
        public void Create(OrderCreateModel _order)
        {
            // 1️⃣ نجيب الألعاب من الـ Repo
            var games = _gameRepo.GetByIds(_order.GameIds);

            // 2️⃣ نحسب الإجمالي
            var total = games.Sum(g => g.Price);

            // 3️⃣ نعمل Order جديد
            var order = new Order
            {
                UserId = _order.UserId,
                TotalAmount = total,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow
            };

            _orderRepo.Create(order);

            // 4️⃣ نضيف تفاصيل كل لعبة جوه OrderItems
            foreach (var g in games)
            {
                var item = new OrderItem
                {
                    OrderId = order.Id,
                    GameId = g.Id,
                    UnitPrice = g.Price
                };

                _orderItemRepo.Create(item);
            }
        }

        // ✅ حذف
        public void Delete(int id)
        {
            _orderRepo.Delete(id);
        }

        // ✅ إحصائية بعدد الأوردرات حسب الـ Status
        public int CountByStatus(OrderStatus status)
        {
            return _orderRepo.CountByStatus(status);
        }
    }
}