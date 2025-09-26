using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Implementations;
using GameStore.BLL.Services;
using GameStore.DAL.DB;
using GameStore.DAL.Repo.Abstractions;
using GameStore.DAL.Repo.Implementations;
using GameStore.PL.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Db connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
?? "Server=.;Database=GameStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

builder.Services.AddDbContext<GameStoreContext>(options =>
options.UseSqlServer(connectionString, b => b.MigrationsAssembly("GameStore.DAL")));

builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<UiExceptionFilter>();
});
// Cookie Auth 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    //  View path
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Register DAL repos
builder.Services.AddScoped<IUserLibraryRepo, UserLibraryRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();


builder.Services.AddScoped<IGameRepo, GameRepo>();
builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
builder.Services.AddScoped<IGameRepo, GameRepo>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
builder.Services.AddScoped<IUserLibraryService, UserLibraryService>();
var app = builder.Build();

// Auto-migrate + seed
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
else
{
    
    app.UseExceptionHandler("/Home/Error");
}


app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Games}/{action=Index}/{id?}");

app.Run();