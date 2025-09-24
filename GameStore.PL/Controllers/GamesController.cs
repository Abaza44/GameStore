using GameStore.DAL.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.PL.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameStoreContext _context;

        public GamesController(GameStoreContext context)
        {
            _context = context;
        }

        // ✅ هنا تحط الأكشن اللي ذكرته
        public IActionResult Index()
        {
            var games = _context.Games
                .Where(g => g.Status == GameStore.DAL.Enums.GameStatus.Approved) // 🟢 ألعاب Approved بس
                .ToList();

            return View(games); // يودّي الـ result لـ Views/Games/Index.cshtml
        }
    }
}