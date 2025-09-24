using GameStore.BLL.ModelVM.Game;

namespace GameStore.BLL.Services.Abstractions
{
    public interface IGameService
    {
        GameViewModel AddGame(GameCreateModel model);
        GameViewModel? GetById(int id);
        IEnumerable<GameViewModel> GetAll();
        IEnumerable<GameViewModel> GetTop(int n);
        IEnumerable<GameViewModel> GetRecent(int n);
        IEnumerable<GameViewModel> GetByPublisher(int publisherId);
        IEnumerable<GameViewModel> GetApproved();
        IEnumerable<GameViewModel> GetPending();
        IEnumerable<GameViewModel> GetRejected();

        void Update(GameUpdateModel model, int publisherId);
        void Approve(int gameId, int adminId);
        void Reject(int gameId, int adminId, string reason);
    }
}
