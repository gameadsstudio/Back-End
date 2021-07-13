namespace api.Models.Campaign
{
    public class CampaignUpdateDto
    {
        public string Name { get; set; }
        
        public string AgeMin { get; set; }
        
        public string AgeMax { get; set; }
        
        public string Type { get; set; }
        
        public string Status { get; set; }
    }
}
