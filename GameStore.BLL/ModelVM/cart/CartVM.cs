using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.ModelVM.cart
{
    public class CartVM
    {
       
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemVM> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
