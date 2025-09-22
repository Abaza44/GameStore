namespace GameStore.DAL.Entities
{
    public class PublisherBalance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Navigations
        public User User { get; set; } = null!;
    }
}
