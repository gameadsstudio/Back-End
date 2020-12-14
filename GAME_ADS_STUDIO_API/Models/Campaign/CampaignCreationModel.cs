using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Campaign
{
    public class CampaignCreationModel
    {
        [Required]
        public uint Org_id { get; set; }

        [Required]
        public string Cpg_name { get; set; }

        [Required]
        public string Cpg_age_min { get; set; }

        [Required]
        public string Cpg_age_max { get; set; }

        [Required]
        public string Cpg_type { get; set; }

        [Required]
        public string Cpg_status { get; set; }

        [Required]
        public DateTimeOffset Cpg_date_begin { get; set; }

        [Required]
        public DateTimeOffset Cpg_date_end { get; set; }

        [Required]
        public DateTimeOffset Cpg_date_creation { get; set; }

        public DateTimeOffset Cpg_date_update { get; set; }
    }
}
