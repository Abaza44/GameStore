using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repo.Implementations
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly GameStoreContext _context;

        public CategoryRepo(GameStoreContext context)
        {
            _context = context;
        }

        public int GetCategoriesCount()
        {
            return this._context.Categories.Count();
        }

        public IEnumerable<Game> GetGamesCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<Game>();

            return _context.Categories.AsNoTracking().
                Include(c => c.Games).Where(c => c.Name == category).SelectMany(c => c.Games);
        }

        public int GetGamesCountByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return 0;
            return _context.Categories.AsNoTracking().
                Include(c => c.Games).Where(c => c.Name == category).SelectMany(c => c.Games).Count();
        }
    }
}
