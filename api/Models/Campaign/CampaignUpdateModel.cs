using System;

namespace api.Models.Campaign
{
    public class CampaignUpdateModel
    {
        public string CpgName { get; set; }
        public string CpgAgeMin { get; set; }
        public string CpgAgeMax { get; set; }
        public string CpgType { get; set; }
        public string CpgStatus { get; set; }
        public DateTimeOffset CpgDateUpdate { get; set; }
    }
}
