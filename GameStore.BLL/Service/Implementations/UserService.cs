using GameStore.BLL.Service.Abstractions;

using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameStore.BLL.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly GameStoreContext _context;

        public UserService(GameStoreContext context)
        {
            _context = context;
        }

        public User Create(string email, string password, string fullName, UserRole role = UserRole.User)
        {
            if (_context.Users.Any(u => u.Email == email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Email = email,
                FullName = fullName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void Delete(int userId)
        {
            // لو هو الأدمن الأساسي (ID=1) متحذفوش
            if (userId == 1)
                throw new Exception("Cannot delete the primary system administrator");

            var user = _context.Users.Find(userId);
            if (user != null)
            {
                // لو ده آخر Admin في السيستم متحذفوش
                if (user.Role == UserRole.Admin)
                {
                    bool otherAdmins = _context.Users.Any(u => u.Role == UserRole.Admin && u.ID != userId);
                    if (!otherAdmins)
                        throw new Exception("Cannot delete the last remaining Admin!");
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public void Update(int userId, string email, string fullName)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.Email = email;
                user.FullName = fullName;
                user.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                _context.SaveChanges();
            }
        }

        public void UpdateRole(int userId, UserRole role)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.Role = role;
                _context.SaveChanges();
            }
        }

        public User? GetById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.ID == userId);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking().ToList();
        }
    }
}