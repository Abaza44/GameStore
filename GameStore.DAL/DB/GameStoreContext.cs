using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DAL.DB
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=HAGGAG;Database=GameStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<PublisherBalance> PublisherBalances { get; set; }
        public DbSet<PayoutRequest> PayoutRequests { get; set; }

        // عشان اخزن قيم ال Enums as String in DB
        protected override void ConfigureConventions(ModelConfigurationBuilder cb)
        {
            cb.Properties<UserRole>().HaveConversion<string>().HaveMaxLength(32);
            cb.Properties<GameStatus>().HaveConversion<string>().HaveMaxLength(32);
            cb.Properties<PaymentProvider>().HaveConversion<string>().HaveMaxLength(32);
            cb.Properties<PaymentStatus>().HaveConversion<string>().HaveMaxLength(32);
            cb.Properties<OrderStatus>().HaveConversion<string>().HaveMaxLength(32);
            cb.Properties<PayoutStatus>().HaveConversion<string>().HaveMaxLength(32);

            cb.Properties<decimal>().HavePrecision(18, 2);
        }


        protected override void OnModelCreating(ModelBuilder model)
        {
            // ========== User ==========
            model.Entity<User>(b =>
            {
                b.HasKey(x => x.ID);
                b.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                b.Property(x => x.Email).HasMaxLength(256).IsRequired();
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.PasswordHash).IsRequired();
                b.Property(x => x.ProfilePicture).HasMaxLength(500);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                // 1:1 User PublisherBalance
                b.HasOne(x => x.PublisherBalance)
                 .WithOne(x => x.User)
                 .HasForeignKey<PublisherBalance>(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                // 1:* User PayoutRequests
                b.HasMany(x => x.PayoutRequests)
                 .WithOne(x => x.User)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // GAME
            model.Entity<Game>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).HasMaxLength(200).IsRequired();
                b.Property(x => x.DownloadUrl).HasMaxLength(500).IsRequired();
                b.Property(x => x.Description).IsRequired();
                b.Property(x => x.PosterUrl).HasMaxLength(500).IsRequired();
                b.Property(x => x.Price);                  // (18,2) من ال-conventions العامة
                b.Property(x => x.RejectionReason).HasMaxLength(500);
                b.Property(x => x.Count).HasDefaultValue(0);
                b.HasIndex(x => x.Status);
                b.HasIndex(x => x.PublisherId);
                b.HasIndex(x => x.Title);
                b.HasIndex(x => x.Count);

                // المهم هنا:
                b.HasIndex(x => x.CategoryId); // فلترة سريعة بالتصنيف
                b.HasOne(x => x.Category)
                 .WithMany(c => c.Games)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict); // ما تمسحش التصنيف لو عليه ألعاب

                b.HasOne(x => x.Publisher)
                 .WithMany(u => u.PublishedGames)
                 .HasForeignKey(x => x.PublisherId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // CATEGORY
            model.Entity<Category>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(100).IsRequired();
                b.HasIndex(x => x.Name).IsUnique();
                // مفيش UsingEntity هنا خلاص
            });


            // ========== Order ==========
            model.Entity<Order>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.OrderDate).HasDefaultValueSql("GETUTCDATE()");
                b.Property(x => x.TotalAmount); // (18,2) من الـ conventions
                b.HasIndex(x => new { x.UserId, x.OrderDate });

                b.HasOne(x => x.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== OrderItem ==========
            model.Entity<OrderItem>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.UnitPrice); // (18,2) من الـ conventions

                // منع تكرار نفس اللعبة في نفس الأوردر
                b.HasIndex(x => new { x.OrderId, x.GameId }).IsUnique();

                b.HasOne(x => x.Order)
                 .WithMany(o => o.Items)
                 .HasForeignKey(x => x.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Game)
                 .WithMany(g => g.OrderItems)
                 .HasForeignKey(x => x.GameId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== Payment (1:1 مع Order) ==========
            model.Entity<Payment>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.TransactionId).HasMaxLength(100).IsRequired();
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.HasIndex(x => x.OrderId).IsUnique(); // 1:1

                b.HasOne(x => x.Order)
                 .WithOne(o => o.Payment!)
                 .HasForeignKey<Payment>(x => x.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== UserGame ==========
            model.Entity<UserGame>(b =>
            {
                b.HasKey(x => new { x.UserId, x.GameId });

                b.Property(x => x.PurchasedAt).HasDefaultValueSql("GETUTCDATE()");

                b.HasOne(x => x.User)
                 .WithMany(u => u.UserGames)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Game)
                 .WithMany(g => g.UserGames)
                 .HasForeignKey(x => x.GameId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== PublisherBalance (1:1 لكل ناشر) ==========
            model.Entity<PublisherBalance>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.CurrentBalance); // (18,2)
                b.Property(x => x.LastUpdated).HasDefaultValueSql("GETUTCDATE()");
                b.HasIndex(x => x.UserId).IsUnique();

                b.HasOne(x => x.User)
                 .WithOne(u => u.PublisherBalance!)
                 .HasForeignKey<PublisherBalance>(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== PayoutRequest ==========
            model.Entity<PayoutRequest>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Amount); // (18,2)
                b.Property(x => x.TransactionId).HasMaxLength(100);
                b.Property(x => x.Notes).HasMaxLength(500);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.HasIndex(x => new { x.UserId, x.Status });

                b.HasOne(x => x.User)
                 .WithMany(u => u.PayoutRequests)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
