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

        // ---------------- SYNC METHODS ----------------
        public void Create(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public void Delete(Game game)
        {
            _context.Games.Remove(game);
            _context.SaveChanges();
        }

        public void Update(Game game)
        {
            var oldGame = _context.Games.Find(game.Id);

            if (oldGame != null)
            {
                if (!string.IsNullOrEmpty(game.Title)) oldGame.Title = game.Title;
                if (game.Price != 0) oldGame.Price = game.Price;
                if (!string.IsNullOrEmpty(game.PosterUrl)) oldGame.PosterUrl = game.PosterUrl;
                if (!string.IsNullOrEmpty(game.Description)) oldGame.Description = game.Description;

                // تحديث الحالة وسبب الرفض
                oldGame.Status = game.Status;
                oldGame.RejectionReason = game.RejectionReason;

                oldGame.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public int GetGamesCount() => _context.Games.AsNoTracking().Count();
        public int GetApprovedCount() => _context.Games.AsNoTracking().Count(g => g.Status == Enums.GameStatus.Approved);
        public int GetPendingCount() => _context.Games.AsNoTracking().Count(g => g.Status == Enums.GameStatus.Pending);
        public int GetRejectedCount() => _context.Games.AsNoTracking().Count(g => g.Status == Enums.GameStatus.Rejected);

        public IEnumerable<Game> GetAllGames() => _context.Games.AsNoTracking().ToList();
        public IEnumerable<Game> GetTopGames(int n)
        {
            n = Math.Min(3, GetGamesCount());
            return _context.Games.AsNoTracking().Where(a=>a.Status==Enums.GameStatus.Approved)
                   .OrderByDescending(g => g.Count)
                   .Take(n)
                   .ToList();
        }
        public IEnumerable<Game> GetRecentGames(int n)
        {
            n = Math.Min(3, GetGamesCount());
            return _context.Games.AsNoTracking().Where(a => a.Status == Enums.GameStatus.Approved)
                   .OrderByDescending(g => g.CreatedAt)
                   .Take(n)
                   .ToList();
        }
        public IEnumerable<Game> GetApprovedGames() => _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Approved).ToList();
        public IEnumerable<Game> GetPendingGames() => _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Pending).ToList();
        public IEnumerable<Game> GetRejectedGames() => _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Rejected).ToList();
        public IEnumerable<Game> GetPublisherGames(int publisherId) => _context.Games.AsNoTracking().Where(g => g.PublisherId == publisherId).ToList();

        public Game? GetById(int id) => _context.Games.AsNoTracking().FirstOrDefault(g => g.Id == id);
        public Game? GetbyName(string name) => _context.Games.AsNoTracking().FirstOrDefault(g => g.Title == name);

        // ---------------- ASYNC METHODS ----------------
        public async Task CreateAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Game game)
        {
            var oldGame = await _context.Games.FindAsync(game.Id);

            if (oldGame != null)
            {
                if (!string.IsNullOrEmpty(game.Title)) oldGame.Title = game.Title;
                if (game.Price != 0) oldGame.Price = game.Price;
                if (!string.IsNullOrEmpty(game.PosterUrl)) oldGame.PosterUrl = game.PosterUrl;
                if (!string.IsNullOrEmpty(game.Description)) oldGame.Description = game.Description;

                oldGame.Status = game.Status;
                oldGame.RejectionReason = game.RejectionReason;

                oldGame.UpdatedAt = DateTime.UtcNow;
                _context.Games.Update(oldGame);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetGamesCountAsync() => await _context.Games.AsNoTracking().CountAsync();
        public async Task<int> GetApprovedCountAsync() => await _context.Games.AsNoTracking().CountAsync(g => g.Status == Enums.GameStatus.Approved);
        public async Task<int> GetPendingCountAsync() => await _context.Games.AsNoTracking().CountAsync(g => g.Status == Enums.GameStatus.Pending);
        public async Task<int> GetRejectedCountAsync() => await _context.Games.AsNoTracking().CountAsync(g => g.Status == Enums.GameStatus.Rejected);

        public async Task<IEnumerable<Game>> GetAllGamesAsync() => await _context.Games.AsNoTracking().ToListAsync();
        public async Task<IEnumerable<Game>> GetTopGamesAsync(int n)
        {
            n = Math.Min(n, await GetGamesCountAsync());
            return await _context.Games.AsNoTracking().Where(a => a.Status == Enums.GameStatus.Approved)
                   .OrderByDescending(g => g.Count)
                   .Take(n)
                   .ToListAsync();
        }
        public async Task<IEnumerable<Game>> GetRecentGamesAsync(int n)
        {
            n = Math.Min(n, await GetGamesCountAsync());
            return await _context.Games.AsNoTracking().Where(a => a.Status == Enums.GameStatus.Approved)
                   .OrderByDescending(g => g.CreatedAt)
                   .Take(n)
                   .ToListAsync();
        }
        public async Task<IEnumerable<Game>> GetApprovedGamesAsync() => await _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Approved).ToListAsync();
        public async Task<IEnumerable<Game>> GetPendingGamesAsync() => await _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Pending).ToListAsync();
        public async Task<IEnumerable<Game>> GetRejectedGamesAsync() => await _context.Games.AsNoTracking().Where(g => g.Status == Enums.GameStatus.Rejected).ToListAsync();
        public async Task<IEnumerable<Game>> GetPublisherGamesAsync(int publisherId) => await _context.Games.AsNoTracking().Where(g => g.PublisherId == publisherId).ToListAsync();

        public async Task<Game?> GetByIdAsync(int id) => await _context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
        public async Task<Game?> GetByNameAsync(string name) => await _context.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Title == name);
        public async Task<IEnumerable<Game>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Games
                                 .AsNoTracking()
                                 .Where(g => ids.Contains(g.Id))
                                 .ToListAsync();
        }

        public IEnumerable<Game> GetByIds(IEnumerable<int> ids)
        {
            return _context.Games
                .Where(g => ids.Contains(g.Id))
                .AsNoTracking()
                .ToList();
        }
    }
}