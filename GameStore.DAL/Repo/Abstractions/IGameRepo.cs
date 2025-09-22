using GameStore.DAL.Entities;

namespace GameStore.DAL.Repo.Abstractions
{
    public interface IGameRepo
    {

        //CRUD Operations
        void Create(Game game);
        void Delete(Game game);
        void Update(Game game);

        Game? GetById(int id);
        Game? GetbyName(string name);

        IEnumerable<Game> GetAllGames();//For Admin Dashboard
        IEnumerable<Game> GetTopGames(int n);
        IEnumerable<Game> GetRecentGames(int n);
        IEnumerable<Game> GetApprovedGames();         // للعرض العام
        IEnumerable<Game> GetPendingGames();          // قائمة المراجعة للأدمن
        IEnumerable<Game> GetRejectedGames();
        IEnumerable<Game> GetPublisherGames(int publisherId);


        int GetGamesCount();
        int GetApprovedCount();
        int GetPendingCount();
        int GetRejectedCount();
    }
}
