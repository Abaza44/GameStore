using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace GameStore.DAL.Repo.Implementations
{
    public class GameRepo : IGameRepo
    {
        private readonly GameStoreContext _context; 
        public GameRepo(GameStoreContext context)
        {
            _context = context;
        }

        public void Create(Game game)
        {
            this._context.Games.Add(game);
            _ = this._context.SaveChanges();
        }
        public void Delete(Game game)
        {
            this._context.Games.Remove(game);
            this._context.SaveChanges();
        }
        public void Update(Game game)
        {
            var oldGame = this._context.Games.Find(game.Id);

            if (oldGame != null)
            {
                if (!string.IsNullOrEmpty(game.Title)) oldGame.Title = game.Title;
                if (game.Price != 0) oldGame.Price = game.Price;
                if (!string.IsNullOrEmpty(game.PosterUrl)) oldGame.PosterUrl = game.PosterUrl;
                if (!string.IsNullOrEmpty(game.Description)) oldGame.Description = game.Description;

                // ✅ Ensure status and rejection reason are updated
                oldGame.Status = game.Status;
                oldGame.RejectionReason = game.RejectionReason;

                oldGame.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }


        public int GetGamesCount()
        {
            return this._context.Games.AsNoTracking().Count();
        }
        public int GetApprovedCount()
        {
            return _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Approved).Count();
        }
        public int GetPendingCount()
        {
            return _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Pending).Count();
        }
        public int GetRejectedCount()
        {
            return _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Rejected).Count();
        }


        public IEnumerable<Game> GetAllGames()
        {
            return this._context.Games.AsNoTracking().ToList();
        }
        public IEnumerable<Game> GetTopGames(int n)
        {
            n = Math.Min(n, this.GetGamesCount());
            var games = this._context.Games.AsNoTracking()
            .OrderByDescending(g => g.Count)
            .Take(n)
            .ToList();

            return games;
        }
        public IEnumerable<Game> GetRecentGames(int n)
        {
            n = Math.Min(n, this.GetGamesCount());
            var games = this._context.Games.AsNoTracking()
            .OrderByDescending(g => g.CreatedAt)
            .Take(n)
            .ToList();

            return games;
        }
        public IEnumerable<Game> GetApprovedGames()
        {
            return _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Approved).ToList();
        }
        public IEnumerable<Game> GetPendingGames()
        {
            return _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Pending).ToList();
        }
        public IEnumerable<Game> GetRejectedGames()
        {
            return _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Rejected).ToList();
        }
        public IEnumerable<Game> GetPublisherGames(int publisherId)
        {
            return _context.Games.AsNoTracking().Where(g => g.PublisherId == publisherId).ToList();
        }


        public Game? GetById(int id)
        {
            return _context.Games.AsNoTracking().Where(g => g.Id == id).FirstOrDefault();
        }
        public Game? GetbyName(string name)
        {
            var game = this._context.Games.AsNoTracking().FirstOrDefault(g => g.Title == name);
            return game!;
        }
    }
}
