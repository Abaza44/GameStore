using GameStore.BLL.ModelVM.Game;
using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;
<<<<<<< HEAD
using MVCproject3tier.BLL.Helper;
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> 908895a1a07d068accb4ce025a9a3c4b98743649

namespace GameStore.BLL.Service.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGameRepo _gameRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IUserRepo _userRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IUserLibraryRepo _userLibraryRepo;
        //private readonly GameStoreContext _context;

        //public GameService(GameStoreContext context)
        //{
        //    _context = context;
        //}

        public GameService(IGameRepo gameRepo, ICategoryRepo categoryRepo, IUserRepo userRepo,
                   IOrderItemRepo orderItemRepo, IUserLibraryRepo userLibraryRepo)
        {
            _gameRepo = gameRepo;
            _categoryRepo = categoryRepo;
            _userRepo = userRepo;
            _orderItemRepo = orderItemRepo;
            _userLibraryRepo = userLibraryRepo;
        }

        public GameViewModel AddGame(GameCreateModel model)
        {
            var PosterUrlPath = Upload.UploadFile("Files", model.PosterUrl);
            var game = new Game
            {
                CategoryId = model.CategoryId,
                PublisherId = model.PublisherId,
                Title = model.Title,
                Description = model.Description,
                PosterUrl = PosterUrlPath,
                Price = model.Price,
                DownloadUrl = model.DownloadUrl,
                Status = GameStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _gameRepo.Create(game);

            return MapToView(game);
        }

        public GameViewModel? GetById(int id)
        {
            var game = _gameRepo.GetById(id);
            return game == null ? null : MapToView(game);
        }

        public IEnumerable<GameViewModel> GetAll() =>
            _gameRepo.GetAllGames().Select(MapToView);

        public IEnumerable<GameViewModel> GetTop(int n) =>
            _gameRepo.GetTopGames(n).Select(MapToView);

        public IEnumerable<GameViewModel> GetRecent(int n) =>
            _gameRepo.GetRecentGames(n).Select(MapToView);

        public IEnumerable<GameViewModel> GetByPublisher(int publisherId) =>
            _gameRepo.GetPublisherGames(publisherId).Select(MapToView);

        public IEnumerable<GameViewModel> GetApproved() =>
            _gameRepo.GetApprovedGames().Select(MapToView);

        public IEnumerable<GameViewModel> GetPending() =>
            _gameRepo.GetPendingGames().Select(MapToView);

        public IEnumerable<GameViewModel> GetRejected() =>
            _gameRepo.GetRejectedGames().Select(MapToView);

        public void Update(GameUpdateModel model, int publisherId)
        {
            var game = _gameRepo.GetById(model.Id);
            if (game == null || game.PublisherId != publisherId)
                throw new UnauthorizedAccessException("Not allowed");

            if (!string.IsNullOrEmpty(model.Title)) game.Title = model.Title;
            if (!string.IsNullOrEmpty(model.Description)) game.Description = model.Description;
            if (!string.IsNullOrEmpty(model.PosterUrl)) game.PosterUrl = model.PosterUrl;
            if (model.Price.HasValue) game.Price = model.Price.Value;

            game.UpdatedAt = DateTime.UtcNow;
            _gameRepo.Update(game);
        }

        public void Approve(int gameId, int adminId)
        {
            var game = _gameRepo.GetById(gameId) ?? throw new KeyNotFoundException();
            game.Status = GameStatus.Approved;
            game.RejectionReason = null;
            _gameRepo.Update(game);
        }

        public void Reject(int gameId, int adminId, string reason)
        {
            var game = _gameRepo.GetById(gameId) ?? throw new KeyNotFoundException();
            game.Status = GameStatus.Rejected;
            game.RejectionReason = reason;
            _gameRepo.Update(game);
        }

        private GameViewModel MapToView(Game g)
        {
            return new GameViewModel
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                PosterUrl = g.PosterUrl,
                Price = g.Price,
                Count = g.Count,
                CategoryName = g.Category?.Name ?? "",
                PublisherName = g.Publisher?.FullName ?? "",
                Status = g.Status,  
                CreatedAt = g.CreatedAt
            };
        }

        public void DeleteGame(int gameId, int currentUserId, bool isCurrentUserAdmin)
        {
            var game = _gameRepo.GetById(gameId) ?? throw new KeyNotFoundException("Game not found.");

            if (!isCurrentUserAdmin && game.PublisherId != currentUserId)
                throw new UnauthorizedAccessException("You do not have permission to delete this game.");

            if (_orderItemRepo.ExistsByGameId(gameId) || _userLibraryRepo.ExistsByGameId(gameId))
                throw new InvalidOperationException("Cannot delete a game that has purchases or exists in users' libraries.");

            _gameRepo.Delete(game);
        }
    }
}
