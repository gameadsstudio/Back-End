using System;
using System.ComponentModel.DataAnnotations;

namespace game_ads_studio_api.Models.Campaign
{
    public class CampaignCreationModel
    {
        [Required]
        public uint OrgId { get; set; }

        [Required]
        public string CpgName { get; set; }

        [Required]
        public string CpgAgeMin { get; set; }

        [Required]
        public string CpgAgeMax { get; set; }

        [Required]
        public string CpgType { get; set; }

        [Required]
        public string CpgStatus { get; set; }

        [Required]
        public DateTimeOffset CpgDateBegin { get; set; }

        [Required]
        public DateTimeOffset CpgDateEnd { get; set; }

        [Required]
        public DateTimeOffset CpgDateCreation { get; set; }

        public DateTimeOffset CpgDateUpdate { get; set; }
    }
}
