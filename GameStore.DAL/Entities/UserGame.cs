namespace GameStore.DAL.Entities
{
    public class UserGame
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
        

        // Navigations
        public User User { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
