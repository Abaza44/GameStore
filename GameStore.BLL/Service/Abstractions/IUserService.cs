using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Service.Abstractions
{
    public interface IUserService
    {
        void Create(string email, string password, string fullName, UserRole role);
        void Delete(int userId);
        void Update(int userId, string email, string password, string fullName);
        User? GetById(int userId);
        IEnumerable<User> GetUsers();
    }
}
