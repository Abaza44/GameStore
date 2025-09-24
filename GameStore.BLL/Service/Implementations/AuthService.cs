using GameStore.BLL.Services.Abstractions;
using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameStore.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly GameStoreContext _context;

        public AuthService(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(string fullName, string email, string password, UserRole role = UserRole.User)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new Exception("This E-mail was Register");

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hash,
                Role = role
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