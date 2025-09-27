using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace GameStore.DAL.Repo.Implementations
{
    public class UserLibraryRepo : IUserLibraryRepo
    {
        private readonly GameStoreContext _context;

        public UserLibraryRepo(GameStoreContext context)
        {
            _context = context;
        }

        private void Create(UserGame userGame)
        {
            _context.UserGames.Add(userGame);
            _context.SaveChanges();
        }

        public IEnumerable<Game> GetUserGames(int userId)
        {
             return this._context.UserGames.AsNoTracking().Where(ug => ug.UserId == userId)
                           .Include(ug => ug.Game)
                           .Select(ug => ug.Game)
                           .ToList();
        }

        public IEnumerable<Game> GetUserGameswithCategory(int userId)
        {
            return this._context.UserGames.AsNoTracking().Where(ug => ug.UserId == userId)
                          .Include(ug => ug.Game)
                          .ThenInclude(g => g.Category)
                          .Select(ug => ug.Game)
                          .ToList();
        }

        public IEnumerable<int> GetOwnedGameIds(int userId)
        {
            return this._context.UserGames.AsNoTracking().Where(ug => ug.UserId == userId)
                .Select(ug => ug.GameId).ToList();
        }

        public bool OwnsGame(int userId, int gameId)
        {
            return this._context.UserGames.AsNoTracking().Any(ug => ug.UserId == userId && ug.GameId == gameId);
        }
        public bool ExistsByGameId(int gameId) => _context.UserGames.Any(x => x.GameId == gameId);
        public void AddOwnership(int userId, int gameId)
        {
            if (!OwnsGame(userId, gameId))
            {
                var userGame = new UserGame
                {
                    UserId = userId,
                    GameId = gameId,
                    PurchasedAt = DateTime.UtcNow
                };
                Create(userGame);
            }
        }
        
    }
}
