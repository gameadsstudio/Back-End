using System;

namespace api.Models.Campaign
{
    public class CampaignModel
    {
        public Guid Id { get; set; }
        public uint OrgId { get; set; }
        public string CpgName { get; set; }
        public string CpgAgeMin { get; set; }
        public string CpgAgeMax { get; set; }
        public string CpgType { get; set; }
        public string CpgStatus { get; set; }
        public DateTimeOffset CpgDateBegin { get; set; }
        public DateTimeOffset CpgDateEnd { get; set; }
        public DateTimeOffset CpgDateCreation { get; set; }
        public DateTimeOffset CpgDateUpdate { get; set; }
    }
}
