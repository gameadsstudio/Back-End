using game_ads_studio_api.Models.Campaign;

namespace game_ads_studio_api.Repositories.Campaign
{
    public interface ICampaignRepository
    {
        int AddNewCampaign(CampaignModel campaign);
        int UpdateCampaign(CampaignModel updatedCampaign, CampaignModel targetCampaign);
        int DeleteCampaign(CampaignModel campaign);
    }
}
