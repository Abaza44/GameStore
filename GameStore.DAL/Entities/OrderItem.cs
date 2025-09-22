
namespace GameStore.DAL.Entities
{
    public class OrderItem
    {
        public long Id { get; set; }
        public int OrderId { get; set; }
        public int GameId { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigations
        public Order Order { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
