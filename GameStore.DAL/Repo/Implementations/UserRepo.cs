using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;
using GameStore.DAL.DB;
using Microsoft.EntityFrameworkCore;



namespace GameStore.DAL.Repo.Implementations
{
    public class UserRepo : IUserRepo
    {
        private readonly GameStoreContext _context;

        public UserRepo(GameStoreContext context)
        {
            _context = context;
        }

        public void Create(User user)
        {
            this._context.Users.Add(user);
            this._context.SaveChanges();
        }
        public bool Delete(User user)
        {
            this._context.Users.Remove(user);
            return this._context.SaveChanges() > 0;
        }
        public void Update(User Updateduser)
        {
            var olduser = this._context.Users.FirstOrDefault(u => u.ID == Updateduser.ID);

            if (olduser != null)
            {
                olduser.FullName = Updateduser.FullName;
                olduser.PasswordHash = Updateduser.PasswordHash;
                olduser.ProfilePicture = Updateduser.ProfilePicture;
                olduser.UpdatedAt = DateTime.UtcNow;
                this._context.SaveChanges();
            }
            else
            {
                throw new Exception("User not found");
            }
        }


        public bool EmailExists(string email)
        {
            var user = this._context.Users.FirstOrDefault(u => u.Email == email);

            return user != null;
        }

        //for admin dashboard
        public IEnumerable<User> GetAllPublishers()
        {
           
            try
            {
                return this._context.Users.AsNoTracking().Where(u => u.Role == Enums.UserRole.Publisher).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("Error retrieving publishers", ex);
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return this._context.Users.AsNoTracking().Where(u => u.Role == Enums.UserRole.User).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Users", ex);
            }
        }

        //If the Email Exist in DB return the user otherwise return null use in Validation
        public User? GetByEmail(string email)
        {
            return this._context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email);
        }
        public User? GetById(int id)
        {
            return this._context.Users.AsNoTracking().FirstOrDefault(u => u.ID == id);
        }


        public PublisherBalance? GetPublisherBalance(int userId)
        {
            return this._context.PublisherBalances.AsNoTracking().Include(pb => pb.User)
                        .Where(u => u.User.Role == Enums.UserRole.Publisher && u.UserId == userId).FirstOrDefault();
        }


        //For Admin Dashboard
        public IEnumerable<PublisherBalance> GetPublishersBalances()
        {
            return this._context.PublisherBalances.AsNoTracking().Include(pb => pb.User)
                        .Where(u => u.User.Role == Enums.UserRole.Publisher)
                        .ToList();
        }

        

        public int GetUsersNumber()
        {
            int count = this._context.Users.Count(u => u.Role == Enums.UserRole.User);
            return count;
        }
        public int GetPublishersNumber()
        {
            int count = this._context.Users.Count(u => u.Role == Enums.UserRole.Publisher);
            return count;
        }
    }
}
