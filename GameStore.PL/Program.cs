//using GameStore.BLL.Service.Abstractions;
//using GameStore.BLL.Service.Implementations;
//using GameStore.BLL.Services.Abstractions;
//using GameStore.BLL.Services.Implementations;
//using GameStore.DAL.DB;
//using GameStore.DAL.Repo.Abstractions;
//using GameStore.DAL.Repo.Implementations;
//using Microsoft.EntityFrameworkCore;

//namespace GameStore.PL
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllersWithViews();
//            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

//            builder.Services.AddDbContext<GameStoreContext>(options =>
//            options.UseSqlServer(connectionString));

//            builder.Services.AddScoped<IUserRepo, UserRepo>();
//            builder.Services.AddScoped<IGameRepo, GameRepo>();
//            builder.Services.AddScoped<IUserLibraryRepo, UserLibraryRepo>();
//            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
//            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
//            builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();

//            builder.Services.AddScoped<IUserService, UserService>();
//            builder.Services.AddScoped<IGameService, GameService>();
//            builder.Services.AddScoped<IOrderService, OrderService>();  

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthorization();

//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=Home}/{action=Index}/{id?}");

//            app.Run();
//        }
//    }
//}




using Microsoft.EntityFrameworkCore;
using GameStore.DAL.DB;
using GameStore.BLL.Services;
using GameStore.DAL.Repo.Implementations;

class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<GameStoreContext>()
            .UseSqlServer("Server=DESKTOP-R33EMI1\\SQLEXPRESS01;Database=GameStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True")
            .Options;

        using var context = new GameStoreContext(options);

        var orderRepo = new OrderRepo(context);
        var orderItemRepo = new OrderItemRepo(context);
        var gameRepo = new GameRepo(context);

        var cartService = new CartService(orderRepo, orderItemRepo, gameRepo);

        int userId = 1;   // your existing user
        int game1Id = 1;  // your first existing game
        int game2Id = 2;  // another game

        Console.WriteLine("=== Step 1: Add first game ===");
        cartService.AddToCart(userId, game1Id);
        PrintCart(cartService.GetCart(userId));

        Console.WriteLine("=== Step 2: Add second game ===");
        cartService.AddToCart(userId, game2Id);
        PrintCart(cartService.GetCart(userId));

        Console.WriteLine("=== Step 3: Remove first game ===");
        cartService.RemoveFromCart(userId, game1Id);
        PrintCart(cartService.GetCart(userId));
    }

    static void PrintCart(GameStore.BLL.ModelVM.cart.CartVM cart)
    {
        Console.WriteLine($"Cart for User {cart.UserId}, Total: {cart.Total}");
        if (!cart.Items.Any())
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        foreach (var item in cart.Items)
        {
            Console.WriteLine($" - {item.GameId}: {item.Title} (${item.Price})");
        }
    }
}

