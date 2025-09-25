using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Implementations;
using GameStore.DAL.DB;
using GameStore.DAL.Repo.Abstractions;
using GameStore.DAL.Repo.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using GameStore.PL.Filters;

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
// Cookie Auth (سجّلها مرة واحدة فقط)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    // خليه يروّح على Action حقيقية مش View path
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Register DAL repos
builder.Services.AddScoped<IUserLibraryRepo, UserLibraryRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();

// موجودة مسبقًا لكن للتذكير:
builder.Services.AddScoped<IGameRepo, GameRepo>();
builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>(); 
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

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
    // نستخدم نفس صفحة الخطأ في الديف لعرض التفاصيل بشكل منسق
    app.UseExceptionHandler("/Home/Error");
}

// صفحة للأكواد (404/403/500...)
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();