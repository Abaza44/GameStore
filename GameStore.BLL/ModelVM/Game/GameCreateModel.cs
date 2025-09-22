using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.ModelVM.Game
{
    public class GameCreateModel
    {
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PosterUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public string DownloadUrl { get; set; } = null!;
    }
}
