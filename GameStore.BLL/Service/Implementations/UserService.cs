using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Repo.Abstractions;


namespace GameStore.BLL.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public void Create(string email, string password, string fullName, UserRole role)
        {
            var user = new User
            {
                Email = email,
                PasswordHash = password, // In a real application, ensure to hash the password
                FullName = fullName,
                Role = role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _userRepo.Create(user);
        }

        public void Delete(int userId)
        {
            var user = _userRepo.GetById(userId);
            if (user != null)
            {
                _userRepo.Delete(user);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public User? GetById(int userId)
        {
            return _userRepo.GetById(userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public void Update(int userId, string email, string password, string fullName)
        {
            var user = new User
            {
                Email = email,
                PasswordHash = password, // In a real application, ensure to hash the password
                FullName = fullName
            };

            _userRepo.Update(user);
        }
    }
}
