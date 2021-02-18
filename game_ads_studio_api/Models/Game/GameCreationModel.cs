using System;
using System.ComponentModel.DataAnnotations;

namespace game_ads_studio_api.Models.Game
{
    public class GameCreationModel
    {
        [Required]
        public uint OrgId { get; set; }

        [Required]
        public uint MediaId { get; set; }

        [Required]
        public string GameName { get; set; }

        [Required]
        public string GameStatus { get; set; }

        [Required]
        public DateTimeOffset GameDateLaunch { get; set; }

        [Required]
        public DateTimeOffset GameDateCreation { get; set; }

        public DateTimeOffset GameDateUpdate { get; set; }
    }
}
