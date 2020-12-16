using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GAME_ADS_STUDIO_API.Models.Campaign
{
    public class CampaignCreationModel
    {
        [Required]
        public uint org_id { get; set; }

        [Required]
        public string cpg_name { get; set; }

        [Required]
        public string cpg_type { get; set; }
    }
}
