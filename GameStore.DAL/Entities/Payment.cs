using GameStore.DAL.Enums;

namespace GameStore.DAL.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentProvider Provider { get; set; }
        public string TransactionId { get; set; } = null!;
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigations
        public Order Order { get; set; } = null!;
    }
}
