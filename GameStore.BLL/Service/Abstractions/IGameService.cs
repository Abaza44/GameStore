using GameStore.BLL.ModelVM.Game;

namespace GameStore.BLL.Service.Abstractions
{
    public interface IGameService
    {
        // ===== Sync Methods (قديمة) =====
        GameViewModel AddGame(GameCreateModel model);
        GameViewModel? GetById(int id);
        IEnumerable<GameViewModel> GetAll();
        IEnumerable<GameViewModel> GetTop(int n);
        IEnumerable<GameViewModel> GetRecent(int n);
        IEnumerable<GameViewModel> GetByPublisher(int publisherId);
        IEnumerable<GameViewModel> GetApproved();
        IEnumerable<GameViewModel> GetPending();
        IEnumerable<GameViewModel> GetRejected();
        void DeleteGame(int gameId, int currentUserId, bool isCurrentUserAdmin);
        void Update(GameUpdateModel model, int publisherId);
        void Approve(int gameId, int adminId);
        void Reject(int gameId, int adminId, string reason);

        // ===== Async Methods (جديدة) =====
        Task<GameViewModel?> GetByIdAsync(int id);
        Task<IEnumerable<GameViewModel>> GetAllAsync();
        Task<IEnumerable<GameViewModel>> GetTopAsync(int n);
        Task<IEnumerable<GameViewModel>> GetRecentAsync(int n);
        Task<IEnumerable<GameViewModel>> GetByPublisherAsync(int publisherId);
        Task<IEnumerable<GameViewModel>> GetApprovedAsync();
        Task<IEnumerable<GameViewModel>> GetPendingAsync();
        Task<IEnumerable<GameViewModel>> GetRejectedAsync();
        GameViewModel AddGame(GameCreateModel model, int publisherId);
    }
}