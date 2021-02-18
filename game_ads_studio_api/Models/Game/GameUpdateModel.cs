using System;

namespace game_ads_studio_api.Models.Game
{
    public class GameUpdateModel
    {
        public string GameName { get; set; }
        public string GameStatus { get; set; }
        public DateTimeOffset GameDateUpdate { get; set; }
    }
}
