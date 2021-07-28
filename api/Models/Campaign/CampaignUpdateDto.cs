using api.Enums.Campaign;

namespace api.Models.Campaign
{
    public class CampaignUpdateDto
    {
        public string Name { get; set; }
        
        public string AgeMin { get; set; }
        
        public string AgeMax { get; set; }

        public CampaignStatus Status { get; set; }
    }
}
