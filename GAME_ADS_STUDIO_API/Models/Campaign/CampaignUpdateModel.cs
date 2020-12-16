using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Models.Campaign
{
    public class CampaignUpdateModel
    {
        public uint org_id { get; set; }
        public string cpg_name { get; set; }
        public int cpg_age_min { get; set; }
        public int cpg_age_max { get; set; }
        public string cpg_type { get; set; }
        public string cpg_status { get; set; }
        public DateTimeOffset cpg_date_begin { get; set; }
        public DateTimeOffset cpg_date_end { get; set; }
    }
}
