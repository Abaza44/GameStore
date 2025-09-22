

namespace GameStore.DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Navigations
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
