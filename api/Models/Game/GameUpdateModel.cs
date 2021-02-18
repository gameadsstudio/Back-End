using System;

namespace api.Models.Game
{
    public class GameUpdateModel
    {
        public string GameName { get; set; }
        public string GameStatus { get; set; }
        public DateTimeOffset GameDateUpdate { get; set; }
    }
}
