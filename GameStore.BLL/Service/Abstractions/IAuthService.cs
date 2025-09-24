using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Service.Abstractions
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string fullName, string email, string password,
                                 DateTime dob, string? profilePicture,
                                 UserRole role = UserRole.User,
                                 bool emailConfirmed = false);

        Task<User?> LoginAsync(string email, string password);
    }
}