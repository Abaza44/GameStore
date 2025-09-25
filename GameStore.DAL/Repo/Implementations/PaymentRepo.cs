using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;

namespace GameStore.DAL.Repo.Implementations
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly GameStoreContext _context;

        public PaymentRepo(GameStoreContext context)
        {
            _context = context;
        }

        // ---------------- Sync Methods ----------------
        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // ---------------- Async Methods ----------------
        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}