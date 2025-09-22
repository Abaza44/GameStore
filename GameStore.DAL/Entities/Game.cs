using GameStore.DAL.Enums;


namespace GameStore.DAL.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PosterUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public string DownloadUrl { get; set; } = null!;
        public GameStatus Status { get; set; } = GameStatus.Pending;
        public string? RejectionReason { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public User Publisher { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();

        public Category Category { get; set; } = null!;
    }
}
