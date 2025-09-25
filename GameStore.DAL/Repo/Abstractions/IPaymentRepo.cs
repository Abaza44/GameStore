using GameStore.DAL.Entities;

namespace GameStore.DAL.Repo.Abstractions
{
    public interface IPaymentRepo
    {
        // ---------------- Sync Methods ----------------
        void Add(Payment payment);
        Task AddAsync(Payment payment);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}