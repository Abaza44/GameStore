using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Service.Abstractions
{
    public interface IUserService
    {
        // إنشاء User جديد
        User Create(string email, string password, string fullName, UserRole role = UserRole.User);

        // حذف User
        void Delete(int userId);

        // تعديل بيانات أساسية
        void Update(int userId, string email, string fullName);

        // تعديل كلمة السر
        void UpdatePassword(int userId, string newPassword);

        // تعديل الرول
        void UpdateRole(int userId, UserRole role);

        // استرجاع مستخدم واحد
        User? GetById(int userId);

        // استرجاع كل المستخدمين
        IEnumerable<User> GetAll();
    }
}