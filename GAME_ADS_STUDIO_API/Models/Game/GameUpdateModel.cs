using System;
namespace GAME_ADS_STUDIO_API.Models.Game
{
    public class GameUpdateModel
    {
        public string Game_name { get; set; }
        public string Game_status { get; set; }
        public DateTimeOffset Game_date_update { get; set; }
    }
}
