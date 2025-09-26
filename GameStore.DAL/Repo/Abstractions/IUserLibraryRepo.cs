using GameStore.DAL.Entities;


namespace GameStore.DAL.Repo.Abstractions
{
    public interface IUserLibraryRepo
    {
        
        bool OwnsGame(int userId, int gameId);

        IEnumerable<Game> GetUserGames(int userId);
        IEnumerable<Game> GetUserGameswithCategory(int userId);
        IEnumerable<int> GetOwnedGameIds(int userId);
        bool ExistsByGameId(int gameId);
        void AddOwnership(int userId, int gameId);
    }
}
