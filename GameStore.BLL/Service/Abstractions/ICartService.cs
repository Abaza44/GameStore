using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.ModelVM.cart;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Service.Abstractions
{
    public interface ICartService
    {
        CartVM GetOrCreateCart(int userId);
        void AddToCart(int userId, int gameId);
        void RemoveFromCart(int userId, int gameId);
        CartVM GetCart(int userId);
        void ClearCart(int userId);

    }
}
