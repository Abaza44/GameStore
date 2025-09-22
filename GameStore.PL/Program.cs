using GameStore.BLL.Service.Abstractions;
using GameStore.BLL.Service.Implementations;
using GameStore.BLL.Services.Abstractions;
using GameStore.BLL.Services.Implementations;
using GameStore.DAL.DB;
using GameStore.DAL.Repo.Abstractions;
using GameStore.DAL.Repo.Implementations;
using Microsoft.EntityFrameworkCore;

namespace GameStore.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

            builder.Services.AddDbContext<GameStoreContext>(options =>
            options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IGameRepo, GameRepo>();
            builder.Services.AddScoped<IUserLibraryRepo, UserLibraryRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IOrderService, OrderService>();  

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}