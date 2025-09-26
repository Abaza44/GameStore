using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Enums;

namespace GameStore.BLL.ModelVM.Game
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PosterUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public string PublisherName { get; set; } = null!;
        public GameStatus Status { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RejectionReason { get; set; }

    }
}
