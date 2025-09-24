using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Implementations;
using GameStore.DAL.DB;
using GameStore.DAL.Repo.Abstractions;
using GameStore.DAL.Repo.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Db connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=.;Database=GameStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("GameStore.DAL")));

builder.Services.AddControllersWithViews();

// Register DAL repos
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IGameRepo, GameRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();

// Register BLL services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Auto-migrate + seed admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    context.Database.Migrate();
    DbInitializer.Seed(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();