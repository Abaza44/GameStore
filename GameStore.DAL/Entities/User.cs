using GameStore.DAL.Enums;

namespace GameStore.DAL.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public ICollection<Game> PublishedGames { get; set; } = new List<Game>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<UserGame> UserGames { get; set; } = new List<UserGame>();
        public PublisherBalance? PublisherBalance { get; set; }
        public ICollection<PayoutRequest> PayoutRequests { get; set; } = new List<PayoutRequest>();

        public override string ToString()
        {
            return $"User: {FullName}, Email: {Email}, Role: {Role}, CreatedAt: {CreatedAt}";
        }
    }
}