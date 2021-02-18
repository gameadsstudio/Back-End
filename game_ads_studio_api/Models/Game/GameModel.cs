using System;

namespace game_ads_studio_api.Models.Game
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public uint OrgId { get; set; }
        public uint MediaId { get; set; }
        public string GameName { get; set; }
        public string GameStatus { get; set; }
        public DateTimeOffset GameDateLaunch { get; set; }
        public DateTimeOffset GameDateCreation { get; set; }
        public DateTimeOffset GameDateUpdate { get; set; }
    }
}
