using System;
namespace GAME_ADS_STUDIO_API.Models.Campaign
{
    public class CampaignModel
    {
        public uint Id { get; set; }
        public uint Org_id { get; set; }
        public string Cpg_name { get; set; }
        public string Cpg_age_min { get; set; }
        public string Cpg_age_max { get; set; }
        public string Cpg_type { get; set; }
        public string Cpg_status { get; set; }
        public DateTimeOffset Cpg_date_begin { get; set; }
        public DateTimeOffset Cpg_date_end { get; set; }
        public DateTimeOffset Cpg_date_creation { get; set; }
        public DateTimeOffset Cpg_date_update { get; set; }
    }
}
