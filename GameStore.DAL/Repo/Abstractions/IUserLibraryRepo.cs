using GameStore.DAL.Entities;


namespace GameStore.DAL.Repo.Abstractions
{
    public interface IUserLibraryRepo
    {
        
        bool OwnsGame(int userId, int gameId);

        IEnumerable<Game> GetUserGames(int userId);
        IEnumerable<int> GetOwnedGameIds(int userId);

        void AddOwnership(int userId, int gameId);
    }
}
