using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.ModelVM.Game
{
    public class UserGameModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string DownloadUrl { get; set; } = null!;
        public string CategoryName { get; set; } = null!;


    }
}
