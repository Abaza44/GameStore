using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameStore.BLL.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly GameStoreContext _context;

        public AuthService(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(string fullName, string email, string password,
                                              DateTime dob, string? profilePicture,
                                              UserRole role = UserRole.User,
                                              bool emailConfirmed = false)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new Exception("This email is already registered");

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hash,
                DateOfBirth = dob,
                ProfilePicture = profilePicture,
                Role = role,
                EmailConfirmed = emailConfirmed
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            bool passwordOk = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return passwordOk ? user : null;
        }
    }
}