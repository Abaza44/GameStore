using GameStore.DAL.Entities;


namespace GameStore.DAL.Repo.Abstractions
{
    public interface ICategoryRepo
    {
        Category GetById(int id);
        IEnumerable<Category> GetAll();
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
    }
}
