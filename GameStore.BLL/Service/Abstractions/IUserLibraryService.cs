using GameStore.BLL.ModelVM.Game;
using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Service.Abstractions
{
    public interface IUserLibraryService
    {
        IEnumerable<UserGameModel> GetUserGameswithCategory(int userId);

    }
}
