//using GameStore.BLL.Services;
//using GameStore.BLL.Service.Implementations;
//using GameStore.DAL.DB;
//using GameStore.DAL.Repo.Implementations;
//using Microsoft.EntityFrameworkCore;

//class Program
//{
//    static void Main(string[] args)
//    {
//        // إعداد DbContext
//        var options = new DbContextOptionsBuilder<GameStoreContext>()
//            .UseSqlServer("Server=.;Database=GameStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True")
//            .Options;
//        ////builder.Services.AddDbContext<GameStoreContext>(options =>
//        ////    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("GameStore.DAL")));
//        using var context = new GameStoreContext(options);

//        // repos
//        var orderRepo = new OrderRepo(context);
//        var orderItemRepo = new OrderItemRepo(context);
//        var gameRepo = new GameRepo(context);

//        // service تجريبي للكارت
//        var cartService = new CartService(orderRepo, orderItemRepo, gameRepo);

//        int userId = 1; // لازم يكون موجود في DB
//        int game1Id = 1;
//        int game2Id = 2;

//        Console.WriteLine("=== Step 1: Add first game ===");
//        cartService.AddToCart(userId, game1Id);
//        PrintCart(cartService.GetCart(userId));

//        Console.WriteLine("=== Step 2: Add second game ===");
//        cartService.AddToCart(userId, game2Id);
//        PrintCart(cartService.GetCart(userId));

//        Console.WriteLine("=== Step 3: Remove first game ===");
//        cartService.RemoveFromCart(userId, game1Id);
//        PrintCart(cartService.GetCart(userId));
//    }

//    static void PrintCart(GameStore.BLL.ModelVM.cart.CartVM cart)
//    {
//        Console.WriteLine($"Cart for User {cart.UserId}, Total: {cart.Total}");
//        if (!cart.Items.Any())
//        {
//            Console.WriteLine("Cart is empty.");
//            return;
//        }

//        foreach (var item in cart.Items)
//        {
//            Console.WriteLine($" - {item.GameId}: {item.Title} (${item.Price})");
//        }
//    }
//}