using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.ModelVM.Order
{
    public class OrderCreateModel
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
