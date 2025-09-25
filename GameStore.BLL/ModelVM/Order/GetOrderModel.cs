using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.ModelVM.Order
{
    public class GetOrderModel
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
    }
}