using GameStore.DAL.Entities;

namespace GameStore.DAL.Repo.Abstractions
{
    public interface IGameRepo
    {
        // ------------------- Sync Methods (قديمة) -------------------
        void Create(Game game);
        void Delete(Game game);
        void Update(Game game);

        Game? GetById(int id);
        Game? GetbyName(string name);

        IEnumerable<Game> GetAllGames();              // For Admin Dashboard
        IEnumerable<Game> GetTopGames(int n);
        IEnumerable<Game> GetRecentGames(int n);
        IEnumerable<Game> GetApprovedGames();         // للعرض العام
        IEnumerable<Game> GetPendingGames();          // قائمة المراجعة للأدمن
        IEnumerable<Game> GetRejectedGames();
        IEnumerable<Game> GetPublisherGames(int publisherId);
        IEnumerable<Game> GetByIds(IEnumerable<int> ids);
        int GetGamesCount();
        int GetApprovedCount();
        int GetPendingCount();
        int GetRejectedCount();


        // ------------------- Async Methods (جديدة) -------------------
        Task CreateAsync(Game game);
        Task DeleteAsync(Game game);
        Task UpdateAsync(Game game);

        Task<Game?> GetByIdAsync(int id);
        Task<Game?> GetByNameAsync(string name);

        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<IEnumerable<Game>> GetTopGamesAsync(int n);
        Task<IEnumerable<Game>> GetRecentGamesAsync(int n);
        Task<IEnumerable<Game>> GetApprovedGamesAsync();
        Task<IEnumerable<Game>> GetPendingGamesAsync();
        Task<IEnumerable<Game>> GetRejectedGamesAsync();
        Task<IEnumerable<Game>> GetPublisherGamesAsync(int publisherId);

        Task<int> GetGamesCountAsync();
        Task<int> GetApprovedCountAsync();
        Task<int> GetPendingCountAsync();
        Task<int> GetRejectedCountAsync();
        Task<IEnumerable<Game>> GetByIdsAsync(IEnumerable<int> ids);
    }
}