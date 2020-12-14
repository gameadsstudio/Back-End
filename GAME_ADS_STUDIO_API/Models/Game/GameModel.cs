using System;
namespace GAME_ADS_STUDIO_API.Models.Game
{
    public class GameModel
    {
        public uint Id { get; set; }
        public uint Org_id { get; set; }
        public uint Media_id { get; set; }
        public string Game_name { get; set; }
        public string Game_status { get; set; }
        public DateTimeOffset Game_date_launch { get; set; }
        public DateTimeOffset Game_date_creation { get; set; }
        public DateTimeOffset Game_date_update { get; set; }
    }
}
