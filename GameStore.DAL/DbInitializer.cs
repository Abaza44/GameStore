using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

public static class DbInitializer
{
    public static void Seed(GameStoreContext context)
    {
        if (!context.Users.Any())
        {
            var admin = new User
            {
                FullName = "System Admin",
                Email = "admin@gamestore.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                EmailConfirmed = true
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Action" },
                new Category { Name = "Adventure" },
                new Category { Name = "Sports" }
            );
            context.SaveChanges();
        }
    }
}