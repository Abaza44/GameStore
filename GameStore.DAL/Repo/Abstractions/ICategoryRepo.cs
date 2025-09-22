using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repo.Abstractions
{
    public interface ICategoryRepo
    {
       IEnumerable<Game> GetGamesCategory(string category);
       int GetGamesCountByCategory(string category);
        int GetCategoriesCount();
    }
}
