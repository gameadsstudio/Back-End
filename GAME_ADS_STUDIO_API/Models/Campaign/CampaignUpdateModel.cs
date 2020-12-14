using System;
namespace GAME_ADS_STUDIO_API.Models.Campaign
{
    public class CampaignUpdateModel
    {
        public string Cpg_name { get; set; }
        public string Cpg_age_min { get; set; }
        public string Cpg_age_max { get; set; }
        public string Cpg_type { get; set; }
        public string Cpg_status { get; set; }
        public DateTimeOffset Cpg_date_update { get; set; }
    }
}
