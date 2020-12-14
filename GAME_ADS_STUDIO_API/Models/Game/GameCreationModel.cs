using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Game
{
    public class GameCreationModel
    {
        [Required]
        public uint Org_id { get; set; }

        [Required]
        public uint Media_id { get; set; }

        [Required]
        public string Game_name { get; set; }

        [Required]
        public string Game_status { get; set; }

        [Required]
        public DateTimeOffset Game_date_launch { get; set; }

        [Required]
        public DateTimeOffset Game_date_creation { get; set; }

        public DateTimeOffset Game_date_update { get; set; }
    }
}
