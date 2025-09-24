using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Services.Abstractions
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string fullName, string email, string password, UserRole role = UserRole.User);
        Task<User?> LoginAsync(string email, string password);
    }
}