using GameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Repo.Abstractions
{
    public interface IUserRepo
    {

        // CRUD Operations
        void Create(User user);
        void Update(User Updateduser);
        bool Delete(User user);


        User? GetById(int id);
        User? GetByEmail(string email);


        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllPublishers();



        int GetUsersNumber();
        int GetPublishersNumber();

        bool EmailExists(string email);


        PublisherBalance? GetPublisherBalance(int userId); // For Publisher Dashboard
        IEnumerable<PublisherBalance> GetPublishersBalances(); //For Admin Dashboard
    }
}
