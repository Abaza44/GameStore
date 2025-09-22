using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.ModelVM.cart
{
    public class CartItemVM
    {
        public int GameId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
