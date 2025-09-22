using GameStore.DAL.Enums;


namespace GameStore.DAL.Entities
{
    public class PayoutRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }           // Publisher
        public decimal Amount { get; set; }
        public PayoutStatus Status { get; set; } = PayoutStatus.Pending;
        public PaymentProvider Provider { get; set; }
        public string? TransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public string? Notes { get; set; }

        // Navigations
        public User User { get; set; } = null!;
    }
}
