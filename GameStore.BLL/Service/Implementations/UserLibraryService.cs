using GameStore.BLL.ModelVM.Game;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Service.Implementations
{
    public class UserLibraryService : IUserLibraryService
    {
        private readonly IUserLibraryRepo _userLibraryrepo;

        public UserLibraryService(IUserLibraryRepo userLibararyRepo)
        {
            _userLibraryrepo = userLibararyRepo;
        }
       

        public IEnumerable<UserGameModel> GetUserGameswithCategory(int userId)
        {
            var games = _userLibraryrepo.GetUserGameswithCategory(userId);
            foreach (var item in games)
            {
                var userGameModel = new UserGameModel();
                userGameModel.Id = item.Id;
                userGameModel.Title = item.Title;
                userGameModel.DownloadUrl = item.DownloadUrl;
                userGameModel.CategoryName = item.Category.Name;
                yield return userGameModel;
            }
        }
    }
}
