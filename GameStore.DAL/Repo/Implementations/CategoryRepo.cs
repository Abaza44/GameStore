using GameStore.DAL.DB;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;

public class CategoryRepo : ICategoryRepo
{
    private readonly GameStoreContext _context;

    public CategoryRepo(GameStoreContext context)
    {
        _context = context;
    }

    public Category GetById(int id) => _context.Categories.Find(id);

    public IEnumerable<Category> GetAll() => _context.Categories.ToList();

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}