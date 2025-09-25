using GameStore.BLL.Service.Abstractions;
using GameStore.DAL.Entities;
using GameStore.DAL.Repo.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _context;
        public CategoryService(ICategoryRepo context)
        {
            _context = context;   
        }

        public void Add(Category category)
        {
            _context.Add(category);
        }

        public void Delete(int id)
        {
            _context.Delete(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.GetAll();
        }

        public Category GetById(int id)
        {
            return _context.GetById(id);
        }

        public void Update(Category category)
        {
            _context.Update(category);
        }
    }
}
